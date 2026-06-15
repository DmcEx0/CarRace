using System.Collections;
using System.Collections.Generic;
using CarRace;
using UnityEngine;

namespace ArcadeVP
{
    public class ArcadeVehicleController : MonoBehaviour
    {
        [SerializeField] private CarInputProvider _carInputProvider;
        public enum groundCheck { rayCast, sphereCaste };
        public enum MovementMode { Velocity, AngularVelocity };
        public MovementMode movementMode;
        public groundCheck GroundCheck;
        public LayerMask drivableSurface;

        public float MaxSpeed, accelaration, turn, gravity = 7f, downforce = 5f;
        [Tooltip("if true : can turn vehicle in air")]
        public bool AirControl = false;
        [Tooltip("if true : vehicle will drift instead of brake while holding space")]
        public bool kartLike = false;
        [Tooltip("turn more while drifting (while holding space) only if kart Like is true")]
        public float driftMultiplier = 1.5f;

        [Header("Ground stability")]
        [Tooltip("Extra distance for ground detection. Higher values keep the car grounded over small bumps.")]
        public float groundCheckDistance = 0.35f;
        [Tooltip("Stability and ramp assist only run inside this close-contact distance. Keep it lower than Ground Check Distance so jumps stay free.")]
        public float groundAssistDistance = 0.2f;
        [Tooltip("Additional force that presses the drive sphere into the current ground normal.")]
        public float groundStickiness = 12f;
        [Tooltip("Maximum velocity allowed away from the ground normal while grounded.")]
        public float maxGroundedUpVelocity = 1.5f;
        [Tooltip("How quickly upward bump impulses are removed while grounded.")]
        public float bumpDamping = 10f;

        [Header("Ramp assist")]
        public bool rampAssist = true;
        [Tooltip("Forward offset from the car body used to sample ramps before the center ground check reaches them.")]
        public float frontProbeOffset = 2.2f;
        public float frontProbeHeight = 0.6f;
        public float frontProbeDistance = 1.6f;
        public float frontProbeRadius = 0.25f;
        [Range(0f, 1f)]
        public float frontNormalBlend = 0.65f;
        [Range(0f, 75f)]
        public float maxRampAssistAngle = 55f;
        public float rampPitchAssist = 6f;

        public Rigidbody rb, carBody;

        [HideInInspector]
        public RaycastHit hit;
        public AnimationCurve frictionCurve;
        public AnimationCurve turnCurve;
        public PhysicsMaterial frictionMaterial;
        [Header("Visuals")]
        public Transform BodyMesh;
        public Transform[] FrontWheels = new Transform[2];
        public Transform[] RearWheels = new Transform[2];
        [HideInInspector]
        public Vector3 carVelocity;

        [Range(0, 10)]
        public float BodyTilt;
        [Header("Audio settings")]
        public AudioSource engineSound;
        [Range(0, 1)]
        public float minPitch;
        [Range(1, 3)]
        public float MaxPitch;
        public AudioSource SkidSound;

        [HideInInspector]
        public float skidWidth;


        private float radius, horizontalInput, verticalInput;
        private Vector3 origin;
        private SphereCollider sphereCollider;
        private RaycastHit frontHit;
        private bool hasFrontGround;

        private void Start()
        {
            sphereCollider = rb.GetComponent<SphereCollider>();
            radius = sphereCollider.radius;
            if (movementMode == MovementMode.AngularVelocity)
            {
                Physics.defaultMaxAngularSpeed = 100;
            }
        }
        private void Update()
        {
            // horizontalInput = Input.GetAxis("Horizontal"); //turning input
            // verticalInput = Input.GetAxis("Vertical"); //accelaration input
            
            horizontalInput = _carInputProvider.SteerInput;
            verticalInput = _carInputProvider.MoveInput;
            Visuals();
            AudioManager();

        }
        public void AudioManager()
        {
            engineSound.pitch = Mathf.Lerp(minPitch, MaxPitch, Mathf.Abs(carVelocity.z) / MaxSpeed);
            if (Mathf.Abs(carVelocity.x) > 10 && grounded())
            {
                SkidSound.mute = false;
            }
            else
            {
                SkidSound.mute = true;
            }
        }


        void FixedUpdate()
        {
            carVelocity = carBody.transform.InverseTransformDirection(carBody.linearVelocity);
            bool isGrounded = grounded();

            if (Mathf.Abs(carVelocity.x) > 0)
            {
                //changes friction according to sideways speed of car
                frictionMaterial.dynamicFriction = frictionCurve.Evaluate(Mathf.Abs(carVelocity.x / 100));
            }


            if (isGrounded)
            {
                bool useGroundAssist = CanUseGroundAssist();
                hasFrontGround = useGroundAssist && rampAssist && TryGetFrontGround(out frontHit);
                Vector3 groundNormal = useGroundAssist ? GetAssistedGroundNormal() : hit.normal;

                if (useGroundAssist)
                {
                    StabilizeGroundContact(groundNormal);
                }

                //turnlogic
                float sign = Mathf.Sign(carVelocity.z);
                float TurnMultiplyer = turnCurve.Evaluate(carVelocity.magnitude / MaxSpeed);
                if (kartLike && Input.GetAxis("Jump") > 0.1f) { TurnMultiplyer *= driftMultiplier; } //turn more if drifting


                if (verticalInput > 0.1f || carVelocity.z > 1)
                {
                    carBody.AddTorque(Vector3.up * horizontalInput * sign * turn * 100 * TurnMultiplyer);
                }
                else if (verticalInput < -0.1f || carVelocity.z < -1)
                {
                    carBody.AddTorque(Vector3.up * horizontalInput * sign * turn * 100 * TurnMultiplyer);
                }



                // mormal brakelogic
                if (!kartLike)
                {
                    if (Input.GetAxis("Jump") > 0.1f)
                    {
                        rb.constraints = RigidbodyConstraints.FreezeRotationX;
                    }
                    else
                    {
                        rb.constraints = RigidbodyConstraints.None;
                    }
                }

                //accelaration logic

                if (movementMode == MovementMode.AngularVelocity)
                {
                    if (Mathf.Abs(verticalInput) > 0.1f && Input.GetAxis("Jump") < 0.1f && !kartLike)
                    {
                        rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, carBody.transform.right * verticalInput * MaxSpeed / radius, accelaration * Time.deltaTime);
                    }
                    else if (Mathf.Abs(verticalInput) > 0.1f && kartLike)
                    {
                        rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, carBody.transform.right * verticalInput * MaxSpeed / radius, accelaration * Time.deltaTime);
                    }
                }
                else if (movementMode == MovementMode.Velocity)
                {
                    if (Mathf.Abs(verticalInput) > 0.1f && Input.GetAxis("Jump") < 0.1f && !kartLike)
                    {
                        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, carBody.transform.forward * verticalInput * MaxSpeed, accelaration / 10 * Time.deltaTime);
                    }
                    else if (Mathf.Abs(verticalInput) > 0.1f && kartLike)
                    {
                        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, carBody.transform.forward * verticalInput * MaxSpeed, accelaration / 10 * Time.deltaTime);
                    }
                }

                // down froce
                rb.AddForce(-(useGroundAssist ? groundNormal : transform.up) * downforce * rb.mass);

                //body tilt
                carBody.MoveRotation(Quaternion.Slerp(carBody.rotation, Quaternion.FromToRotation(carBody.transform.up, groundNormal) * carBody.transform.rotation, 0.12f));
            }
            else
            {
                hasFrontGround = false;

                if (AirControl)
                {
                    //turnlogic
                    float TurnMultiplyer = turnCurve.Evaluate(carVelocity.magnitude / MaxSpeed);

                    carBody.AddTorque(Vector3.up * horizontalInput * turn * 100 * TurnMultiplyer);
                }

                carBody.MoveRotation(Quaternion.Slerp(carBody.rotation, Quaternion.FromToRotation(carBody.transform.up, Vector3.up) * carBody.transform.rotation, 0.02f));
                rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, rb.linearVelocity + Vector3.down * gravity, Time.deltaTime * gravity);
            }

        }
        private Vector3 GetAssistedGroundNormal()
        {
            if (!hasFrontGround)
            {
                return hit.normal;
            }

            float rampAngle = Vector3.Angle(frontHit.normal, Vector3.up);
            bool frontSurfaceFacesCar = Vector3.Dot(carBody.transform.forward, frontHit.normal) < -0.05f;

            if (!frontSurfaceFacesCar || rampAngle > maxRampAssistAngle)
            {
                return hit.normal;
            }

            return Vector3.Slerp(hit.normal, frontHit.normal, frontNormalBlend).normalized;
        }

        private bool CanUseGroundAssist()
        {
            return hit.collider != null && hit.distance <= radius + Mathf.Max(0f, groundAssistDistance);
        }

        private void StabilizeGroundContact(Vector3 groundNormal)
        {
            float awaySpeed = Vector3.Dot(rb.linearVelocity, groundNormal);
            if (awaySpeed > maxGroundedUpVelocity)
            {
                float dampedAwaySpeed = Mathf.Lerp(awaySpeed, maxGroundedUpVelocity, Mathf.Clamp01(bumpDamping * Time.fixedDeltaTime));
                rb.linearVelocity -= groundNormal * (awaySpeed - dampedAwaySpeed);
            }

            float speedFactor = MaxSpeed > 0 ? Mathf.Clamp01(rb.linearVelocity.magnitude / MaxSpeed) : 0f;
            rb.AddForce(-groundNormal * Mathf.Max(0f, groundStickiness) * rb.mass * (1f + speedFactor));

            if (hasFrontGround && Vector3.Dot(carBody.transform.forward, frontHit.normal) < -0.05f)
            {
                carBody.AddTorque(-carBody.transform.right * Mathf.Max(0f, rampPitchAssist) * Mathf.Max(0.2f, speedFactor), ForceMode.Force);
            }
        }

        private bool TryGetFrontGround(out RaycastHit frontGroundHit)
        {
            Vector3 probeOrigin = carBody.position
                                  + carBody.transform.forward * frontProbeOffset
                                  + carBody.transform.up * frontProbeHeight;
            return Physics.SphereCast(probeOrigin, Mathf.Max(0.01f, frontProbeRadius), -carBody.transform.up, out frontGroundHit, Mathf.Max(0f, frontProbeDistance), drivableSurface, QueryTriggerInteraction.Ignore);
        }

        public void Visuals()
        {
            //tires
            foreach (Transform FW in FrontWheels)
            {
                FW.localRotation = Quaternion.Slerp(FW.localRotation, Quaternion.Euler(FW.localRotation.eulerAngles.x,
                                   30 * horizontalInput, FW.localRotation.eulerAngles.z), 0.7f * Time.deltaTime / Time.fixedDeltaTime);
                FW.GetChild(0).localRotation = rb.transform.localRotation;
            }
            RearWheels[0].localRotation = rb.transform.localRotation;
            RearWheels[1].localRotation = rb.transform.localRotation;

            //Body
            if (carVelocity.z > 1)
            {
                BodyMesh.localRotation = Quaternion.Slerp(BodyMesh.localRotation, Quaternion.Euler(Mathf.Lerp(0, -5, carVelocity.z / MaxSpeed),
                                   BodyMesh.localRotation.eulerAngles.y, BodyTilt * horizontalInput), 0.4f * Time.deltaTime / Time.fixedDeltaTime);
            }
            else
            {
                BodyMesh.localRotation = Quaternion.Slerp(BodyMesh.localRotation, Quaternion.Euler(0, 0, 0), 0.4f * Time.deltaTime / Time.fixedDeltaTime);
            }


            if (kartLike)
            {
                if (Input.GetAxis("Jump") > 0.1f)
                {
                    BodyMesh.parent.localRotation = Quaternion.Slerp(BodyMesh.parent.localRotation,
                    Quaternion.Euler(0, 45 * horizontalInput * Mathf.Sign(carVelocity.z), 0),
                    0.1f * Time.deltaTime / Time.fixedDeltaTime);
                }
                else
                {
                    BodyMesh.parent.localRotation = Quaternion.Slerp(BodyMesh.parent.localRotation,
                    Quaternion.Euler(0, 0, 0),
                    0.1f * Time.deltaTime / Time.fixedDeltaTime);
                }

            }

        }

        public bool grounded() //checks for if vehicle is grounded or not
        {
            if (sphereCollider == null)
            {
                sphereCollider = rb.GetComponent<SphereCollider>();
            }

            radius = sphereCollider.radius;
            origin = rb.position + radius * Vector3.up;
            var direction = -transform.up;
            var maxdistance = radius + Mathf.Max(0f, groundCheckDistance);

            if (GroundCheck == groundCheck.rayCast)
            {
                if (Physics.Raycast(rb.position, Vector3.down, out hit, maxdistance, drivableSurface))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            else if (GroundCheck == groundCheck.sphereCaste)
            {
                if (Physics.SphereCast(origin, radius + 0.1f, direction, out hit, maxdistance, drivableSurface))
                {
                    return true;

                }
                else
                {
                    return false;
                }
            }
            else { return false; }
        }

        private void OnDrawGizmos()
        {
            //debug gizmos
            radius = rb.GetComponent<SphereCollider>().radius;
            float width = 0.02f;
            if (!Application.isPlaying)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(rb.transform.position + ((radius + width) * Vector3.down), new Vector3(2 * radius, 2 * width, 4 * radius));
                if (GetComponent<BoxCollider>())
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider>().size);
                }

            }

        }

    }
}
