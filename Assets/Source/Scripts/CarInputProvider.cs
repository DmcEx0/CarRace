using UnityEngine;

namespace CarRace
{
    using UnityEngine;

    public class CarInputProvider : MonoBehaviour
    {
        [SerializeField] private string _horizontalAxis = "Horizontal";
        [SerializeField] private string _verticalAxis = "Vertical";
        [SerializeField] private Transform _carTransform;
        [SerializeField] private float _deadZone = 0.15f;
        [SerializeField] private float _angleForBack;

        public float SteerInput { get; private set; }
        public float MoveInput { get; private set; }

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

            // Переводим направление стика в локальные координаты машины
            Vector3 localStick = _carTransform.InverseTransformDirection(stickWorld);

            // Вперёд для любой передней/боковой полусферы, назад только для задней
            MoveInput = localStick.z >= 0f ? magnitude : -magnitude;
            
            // float angle = Mathf.Atan2(localStick.x, localStick.z) * Mathf.Rad2Deg;

            // Поворот берём из локального X
            SteerInput = localStick.x;

            // При движении назад инвертируем руль,
            // чтобы управление ощущалось естественно
            
            Debug.Log(MoveInput);
            
            if (MoveInput < _angleForBack)
                SteerInput = -SteerInput;

            SteerInput = Mathf.Clamp(SteerInput, -1f, 1f);
        }
    }
}