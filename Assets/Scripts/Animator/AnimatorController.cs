namespace RehvidGames.Animator
{
    using System;
    using UnityEngine;

    public class AnimatorController: MonoBehaviour
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

        public void ApplyRootMotion() => _animator.applyRootMotion = true;

        public void DisableRootMotion() => _animator.applyRootMotion = false;
        
        private string GetAnimationNameByHash(int hash) => AnimatorParameter.GetParameterName(hash);
    }
}