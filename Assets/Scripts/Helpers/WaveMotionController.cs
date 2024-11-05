namespace RehvidGames.Helpers
{
    using UnityEngine;

    public class WaveMotionController : MonoBehaviour
    {
        [SerializeField] private float _frequency = 1f;
        [SerializeField] private Vector3 _direction = Vector3.up;
        [SerializeField] private Transform _startPosition;
        
        [SerializeField] private float _maxUpwardDisplacement = 1.5f; 
        [SerializeField] private float _maxDownwardDisplacement = 0.5f;
        
        public bool active;

        private void Update()
        {
            if (!active) return;

            transform.position = GetWavePosition();
        }

        private Vector3 GetWavePosition()
        {
            var waveOffset = Mathf.Sin(Time.time * _frequency);
            var amplitude  = (_maxUpwardDisplacement - _maxDownwardDisplacement) / 2f;
            var midPoint = (_maxUpwardDisplacement + _maxDownwardDisplacement) / 2f;
            
            return _startPosition.position + _direction * (midPoint + waveOffset * amplitude);
        }
    }
}