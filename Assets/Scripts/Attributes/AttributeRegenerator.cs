namespace RehvidGames.Attributes
{
    using System.Collections;
    using Interfaces;
    using UnityEngine;

    public class AttributeRegenerator: MonoBehaviour
    {
        [Tooltip("Rate at which attribute value regenerates over time. Higher values will result in faster regeneration.")]
        [SerializeField] private float _regenRate = 5f;
        private Coroutine _regenCoroutine;

        public void StartRegeneration(IAttribute attribute, System.Action<IAttribute> updateUI)
        {
            if (attribute == null) return;
            
            if (updateUI == null)
            {
                Debug.LogWarning("Update UI action is null. No UI update will occur.");
            }
            
            if (_regenCoroutine == null)
            {
                _regenCoroutine = StartCoroutine(Regenerate(attribute, updateUI));
            }
            else
            {
                Debug.LogWarning("Regeneration is already in progress.");
            }
        }

        public bool IsRegenerationStarted() => _regenCoroutine != null;
        
        public void StopRegeneration()
        {
            if (_regenCoroutine == null) return;
            StopCoroutine(_regenCoroutine);
            _regenCoroutine = null;
        }
        
        private IEnumerator Regenerate(IAttribute attribute, System.Action<IAttribute> updateUI)
        {
            while (attribute.CurrentValue < attribute.MaxValue)
            {
                attribute.CurrentValue += _regenRate * Time.deltaTime; 
                attribute.CurrentValue = Mathf.Clamp(attribute.CurrentValue, 0, attribute.MaxValue);
                updateUI?.Invoke(attribute);
            
                yield return null;
            }
            
            attribute.CurrentValue = attribute.MaxValue;
            updateUI?.Invoke(attribute);
            StopRegeneration();
        }
    }
}