namespace RehvidGames.Attributes.Base
{
    using UnityEngine;

    public abstract class BaseAttribute : MonoBehaviour
    {
        [Header("Attributes")] 
        [Range(0, 100), SerializeField] private float _currentValue;
        [Range(0, 100), SerializeField] private float _maxValue;
        
        public float CurrentValue
        {
            get => _currentValue;
            set => _currentValue = Mathf.Clamp(value, 0, _maxValue);
        }

        public float MaxValue
        {
            get => _maxValue;
            set => _maxValue = _currentValue > value ? _currentValue : value; 
        }
    }
}