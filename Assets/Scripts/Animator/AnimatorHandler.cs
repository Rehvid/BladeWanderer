namespace RehvidGames.Animator
{
    using System;
    using UnityEngine;

    [RequireComponent(typeof(Animator))]
    public class AnimatorHandler: MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            TryGetComponent(out _animator);
        }

        private void Start()
        {
            if (_animator == null)
            {
                throw new MissingComponentException("Animator component is missing on the GameObject.");
            }
        }

        public void SetTrigger(int hash) => _animator.SetTrigger(GetAnimationNameByHash(hash));

        public void SetFloat(int hash, float value) => _animator.SetFloat(GetAnimationNameByHash(hash), value);

        public void SetBool(int hash, bool value) => _animator.SetBool(GetAnimationNameByHash(hash), value);
        
        private string GetAnimationNameByHash(int hash) => AnimatorParameter.GetParameterName(hash);
    }
}