namespace RehvidGames.Attributes
{
    using System.Collections;
    using Interfaces;
    using UnityEngine.Events;
    using UnityEngine;

    public abstract class BaseAttribute : MonoBehaviour, IAttribute
    {
        [Header("Attributes")] 
        [Range(0, 999)] [SerializeField] private float _currentValue;
        [Range(0, 999)] [SerializeField] private float _maxValue;

        [Header("Interpolate Configuration")]
        [Tooltip("Time (in seconds) for how long the interpolation will occur.")]
        [SerializeField] protected float _lerpDuration = 1f;

        [Tooltip("Interval (in seconds) at which the UI will be updated.")] 
        [SerializeField] protected float _uiUpdateInterval = 0.05f;

        [Header("Events")]
        [SerializeField] private UnityEvent<IAttribute> attributeChanged;
        
        protected float interpolatedValue;
        private bool _useInterpolatedValue;

        public float CurrentValue
        {
            get => _currentValue;
            set => _currentValue = Mathf.Clamp(value, 0, _maxValue);
        }

        public float MaxValue
        {
            get => _maxValue;
            set => _maxValue = value;
        }

        public float GetInterpolatedOrRawValue() => _useInterpolatedValue ? interpolatedValue : _currentValue;

        protected void InitUI()
        {
            if (attributeChanged == null) return;
            interpolatedValue = _currentValue;
            InvokeAttributeChangeEvent();
        }

        protected IEnumerator InterpolateAttributeValue()
        {
            _useInterpolatedValue = true;
            var elapsedTime = 0f;
            var lastUIUpdateTime = 0f;

            while (elapsedTime < _lerpDuration)
            {
                elapsedTime += Time.deltaTime;
                interpolatedValue = Mathf.Lerp(interpolatedValue, _currentValue, elapsedTime / _lerpDuration);
                if (elapsedTime - lastUIUpdateTime >= _uiUpdateInterval)
                {
                    InvokeAttributeChangeEvent();
                    lastUIUpdateTime = elapsedTime;
                }

                yield return null;
            }

            interpolatedValue = _currentValue;
            InvokeAttributeChangeEvent();
            _useInterpolatedValue = false;
        }
        
        protected void InvokeAttributeChangeEvent()
        {
            attributeChanged?.Invoke(this);
        }
    }
}