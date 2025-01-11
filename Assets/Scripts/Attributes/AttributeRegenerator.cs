namespace RehvidGames.Attributes
{
    using System.Collections;
    using Base;
    using Managers;
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
            while (CanRegenerate(baseAttribute))
            {
                if (Time.timeScale == 0)
                {
                    StopRegeneration();
                    break;
                }
                baseAttribute.CurrentValue += _regenRate * Time.deltaTime; 
                baseAttribute.CurrentValue = Mathf.Clamp(baseAttribute.CurrentValue, 0, baseAttribute.MaxValue);
                updateUI?.Invoke(baseAttribute);
            
                yield return null;
            }
 
            if (!GameManager.Instance.IsPaused)
            {
                baseAttribute.CurrentValue = baseAttribute.MaxValue; 
                updateUI?.Invoke(baseAttribute);
            }
            StopRegeneration();
        }

        private bool CanRegenerate(BaseAttribute baseAttribute) =>
            baseAttribute.CurrentValue < baseAttribute.MaxValue && Time.timeScale > 0;
    }
}