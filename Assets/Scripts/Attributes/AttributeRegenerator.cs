namespace RehvidGames.Attributes
{
    using System.Collections;
    using Base;
    using Interfaces;
    using UnityEngine;

    public class AttributeRegenerator: MonoBehaviour
    {
        [Tooltip("Rate at which attribute value regenerates over time. Higher values will result in faster regeneration.")]
        [SerializeField] private float _regenRate = 5f;
        private Coroutine _regenCoroutine;

        public void StartRegeneration(BaseAttribute baseAttribute, System.Action<BaseAttribute> updateUI)
        {
            if (baseAttribute == null) return;
            
            if (updateUI == null)
            {
                Debug.LogWarning("Update UI action is null. No UI update will occur.");
            }
            
            if (_regenCoroutine == null)
            {
                _regenCoroutine = StartCoroutine(Regenerate(baseAttribute, updateUI));
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
        
        private IEnumerator Regenerate(BaseAttribute baseAttribute, System.Action<BaseAttribute> updateUI)
        {
            while (baseAttribute.CurrentValue < baseAttribute.MaxValue)
            {
                baseAttribute.CurrentValue += _regenRate * Time.deltaTime; 
                baseAttribute.CurrentValue = Mathf.Clamp(baseAttribute.CurrentValue, 0, baseAttribute.MaxValue);
                updateUI?.Invoke(baseAttribute);
            
                yield return null;
            }
            
            baseAttribute.CurrentValue = baseAttribute.MaxValue;
            updateUI?.Invoke(baseAttribute);
            StopRegeneration();
        }
    }
}