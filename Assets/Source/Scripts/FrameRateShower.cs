using System.Text;
using TMPro;
using UnityEngine;

namespace CarRace
{
    public class FrameRateShower : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private float _delay;

        private StringBuilder _stringBuilder;

        private float _currentTime;

        private void Start()
        {
            _stringBuilder = new StringBuilder();
        }

        private void Update()
        {
            _currentTime += Time.deltaTime;
            
            if (_currentTime >= _delay)
            {
                _currentTime = 0;
                _stringBuilder.Append(Application.targetFrameRate);
                _text.text = _stringBuilder.ToString();
            }
        }
    }
}