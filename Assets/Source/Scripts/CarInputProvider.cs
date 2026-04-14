using System;
using UnityEngine;

namespace CarRace
{
    public class CarInputProvider : MonoBehaviour
    {
        [SerializeField] private string _horizontalAxis = "Horizontal";
        [SerializeField] private string _verticalAxis = "Vertical";
        [SerializeField] private Transform _carTransform;
        [SerializeField] private float _deadZone = 0.15f;

        [Header("Reverse switching")]
        
        [SerializeField, Range(0f, 180f)] private float _enterBackAngle = 120f;
        [SerializeField, Range(0f, 180f)] private float _exitBackAngle = 60f;

        public bool AbsoluteDirectionSteering;

        public float SteerInput { get; private set; }
        public float MoveInput { get; private set; }

        private bool _isReversing;

        private void Start()
        {
            Application.targetFrameRate = 120;
        }

        private void Update()
        {
            float rawX = SimpleInput.GetAxis(_horizontalAxis);
            float rawY = SimpleInput.GetAxis(_verticalAxis);

            Vector3 stickWorld = new Vector3(rawX, 0f, rawY);
            float magnitude = Mathf.Clamp01(stickWorld.magnitude);

            if (magnitude < _deadZone)
            {
                SteerInput = 0f;
                MoveInput = 0f;
                return;
            }
            stickWorld /= magnitude;

            if (AbsoluteDirectionSteering)
            {
                Vector3 localStick = _carTransform.InverseTransformDirection(stickWorld);

                // Угол относительно forward машины:
                // 0 = строго вперед, 180 = строго назад
                float absAngle = Mathf.Abs(Mathf.Atan2(localStick.x, localStick.z) * Mathf.Rad2Deg);

                // Hysteresis:
                // В задний ход входим только если угол явно в задней полусфере.
                // Выходим из заднего хода только если угол снова явно спереди.
                if (_isReversing)
                {
                    if (absAngle < _exitBackAngle)
                        _isReversing = false;
                }
                else
                {
                    if (absAngle > _enterBackAngle)
                        _isReversing = true;
                }

                MoveInput = _isReversing ? -magnitude : magnitude;

                SteerInput = localStick.x;

                // Для естественного руля при движении назад
                // if (_isReversing)
                //     SteerInput = -SteerInput;

                SteerInput = Mathf.Clamp(SteerInput, -1f, 1f);
            }
            else
            {
                MoveInput = rawY;
                SteerInput = rawX;
            }


        }

        
    }
}