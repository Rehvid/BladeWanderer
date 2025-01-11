namespace RehvidGames.Utilities
{
    using UnityEngine;

    public class WaveMotionController : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private float _frequency = 0.5f;
        [SerializeField] private float _maxUpwardDisplacement = 1.5f; 
        [SerializeField] private float _maxDownwardDisplacement = 0.5f;
        
        [Header("Wave parameters")]
        [SerializeField] private Vector3 _direction = Vector3.up;
        
        [SerializeField, Tooltip("Wave start position. A vector added to the direction to determine the base position")] 
        private Vector3 _startPosition;

        public void SetStartPosition(Vector3 position) => _startPosition = position; 
        
        private void Update()
        {
            transform.position = GetWavePosition();
        }
        
        private Vector3 GetWavePosition()
        {
            var waveOffset = Mathf.Sin(Time.time * _frequency);
            var amplitude  = (_maxUpwardDisplacement - _maxDownwardDisplacement) / 2f;
            var midPoint = (_maxUpwardDisplacement + _maxDownwardDisplacement) / 2f;
             
            var waveDisplacement = _direction.normalized * (midPoint + waveOffset * amplitude);
            
            return new Vector3(
                _direction.x != 0 ? _startPosition.x + waveDisplacement.x : _startPosition.x,
                _direction.y != 0 ? Mathf.Max(_maxDownwardDisplacement, _startPosition.y + waveDisplacement.y) : Mathf.Max(_maxDownwardDisplacement, _startPosition.y),
                _direction.z != 0 ? _startPosition.z + waveDisplacement.z : _startPosition.z
            );
        }
    }
}