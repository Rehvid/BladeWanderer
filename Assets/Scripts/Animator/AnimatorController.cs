namespace RehvidGames.Animator
{
    using System;
    using Data.Serializable;
    using UnityEngine;

    public class AnimatorController: MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public void OnPlayAnimation(AnimationData animationData)
        {
            if (!IsValidAnimation(animationData)) return;
            TriggerAnimation(animationData);
        }

        public void OnPlayMultiAnimations(AnimationData[] animationsData)
        {
            foreach (var animationData in animationsData)
            {
                if (IsValidAnimation(animationData))
                {
                    TriggerAnimation(animationData);
                }
            }
        }
        
        public void PlayAnimation(string animationName, AnimatorParameterType parameterType, object value = null)
        {
            var animationData = new AnimationData
            {
                AnimationName = animationName,
                ParameterType = parameterType,
                Value = value
            };

            TriggerAnimation(animationData);
        }
        
        private void TriggerAnimation(AnimationData animationData)
        {
            switch (animationData.ParameterType)
            {
                case AnimatorParameterType.Trigger:
                    _animator.SetTrigger(animationData.AnimationName);
                    break;
                case AnimatorParameterType.Bool:
                    _animator.SetBool(animationData.AnimationName, (bool) animationData.Value);
                    break;
                case AnimatorParameterType.Float:
                    _animator.SetFloat(animationData.AnimationName, (float) animationData.Value);
                    break;
                case AnimatorParameterType.Int:
                    _animator.SetInteger(animationData.AnimationName, (int) animationData.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(animationData.ParameterType), "Invalid Animator Parameter Type.");
            }
        }
        
        private bool IsValidAnimation(AnimationData animationData)
        {
            return _animator != null && animationData != null;
        }
    }
}