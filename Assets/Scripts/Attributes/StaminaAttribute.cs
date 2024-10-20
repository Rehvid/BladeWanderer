namespace RehvidGames.Attributes
{
    using System.Collections;
    using UnityEngine;

    public class StaminaAttribute: BaseAttribute
    {
        [Header("Stamina Configuration")]
        [Tooltip("Rate at which stamina regenerates over time. Higher values will result in faster regeneration.")]
        [SerializeField] private float _staminaRegenRate = 5f;
        
        [Tooltip("Smooth time for the stamina regeneration process. Controls the smoothness of regeneration (lower values make it faster and less smooth)")]
        [SerializeField] private float _smoothTime = 0.3f;
        
        [Tooltip("Delay before the stamina regeneration process begins after stamina is used.")]
        [SerializeField] private float _regenDelay = 1f;
        
        [Tooltip("Interval (in seconds) at which the UI will be updated during stamina regeneration. Lower values will update the UI more frequently but may affect performance.")]
        [SerializeField] private float _uiUpdateStaminaGegenerationInterval = 0.05f;
        
        private Coroutine _regenCoroutine;
        private float _timeSinceLastUIUpdate;
        
        private void Start()
        {
            InitUI();
        }
        
        public void UseStamina(float useStamina)
        {
            if (!HasEnoughStamina(useStamina)) return;
            CurrentValue -= useStamina;
            StartCoroutine(InterpolateAttributeValue());
            
            if (_regenCoroutine != null)
            {
                StopCoroutine(_regenCoroutine);
            }

            _regenCoroutine = StartCoroutine(RegenerateStamina());
        }
        
        public bool HasEnoughStamina(float stamina)
        {
            return CurrentValue - stamina > 0;
        }

        private IEnumerator RegenerateStamina()
        {
            yield return new WaitForSeconds(_regenDelay);
            var currentVelocity = 0f;
            
            while (CurrentValue < MaxValue)
            {
                CurrentValue = Mathf.SmoothDamp(CurrentValue, MaxValue, ref currentVelocity, _smoothTime / _staminaRegenRate);
                interpolatedValue = CurrentValue;
                _timeSinceLastUIUpdate += Time.deltaTime;
                if (_timeSinceLastUIUpdate >= _uiUpdateStaminaGegenerationInterval)
                {
                    InvokeAttributeChangeEvent();
                    _timeSinceLastUIUpdate = 0f;
                }
                yield return null; 
            }
            interpolatedValue = MaxValue; 
            _regenCoroutine = null;
            InvokeAttributeChangeEvent();
        }
    }
}