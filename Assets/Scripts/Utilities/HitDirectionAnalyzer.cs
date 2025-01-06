namespace RehvidGames.Utilities
{
    using System;
    using Animator;
    using Enums;
    using UnityEngine;

    public class HitDirectionAnalyzer
    {
        private const float FrontHitAngleThreshold = 45f;
        private const float CrossProductThresholdY = 0f;
        private const float DotProductThreshold = 0f;
        
        private Vector3 _hitDirectionNormalized;
        private Vector3 _forwardDirectionNormalized;

        public HitDirectionType GetFrontBackDirection(Vector3 hitPosition, Transform damageReceiver)
        {
            SetHitDirectionNormalized(hitPosition, damageReceiver);
            SetForwardDirectionNormalized(damageReceiver);
            
            return CalculateDotProduct() <= DotProductThreshold ? HitDirectionType.Back : HitDirectionType.Front;
        }
        
        public HitDirectionType GetDirectionType(Vector3 hitPosition, Transform damageReceiver)
        {
            SetHitDirectionNormalized(hitPosition, damageReceiver);
            SetForwardDirectionNormalized(damageReceiver);

            if (CalculateDotProduct() <= DotProductThreshold)
            {
                return HitDirectionType.Back;
            }
            
            if (CalculateAngleBetweenDirections() <= FrontHitAngleThreshold)
            {
                return HitDirectionType.Front;
            }
            
            return CalculateCrossProduct().y > CrossProductThresholdY ? HitDirectionType.Right : HitDirectionType.Left;
        }
        
        public int GetAnimatorParameterTypeByHitDirectionType(HitDirectionType directionType)
        {
            return directionType switch
            {
                HitDirectionType.Left => AnimatorParameter.HitDirectionLeft,
                HitDirectionType.Right => AnimatorParameter.HitDirectionRight,
                HitDirectionType.Front => AnimatorParameter.HitDirectionFront,
                HitDirectionType.Back => AnimatorParameter.HitDirectionBack,
                _ => throw new ArgumentOutOfRangeException(nameof(directionType), directionType, null)
            };
        }
        
        private void SetHitDirectionNormalized(Vector3 hitPosition, Transform damageReceiver)
        {
            Vector3 hitDirection = (hitPosition - damageReceiver.position);
            hitDirection.y = 0; 
            hitDirection.Normalize();

            _hitDirectionNormalized = hitDirection;
        }

        private void SetForwardDirectionNormalized(Transform damageReceiver)
        {
            Vector3 forwardDirection = damageReceiver.forward;
            forwardDirection.y = 0;
            forwardDirection.Normalize();

            _forwardDirectionNormalized = forwardDirection;
        }

        private float CalculateDotProduct() => Vector3.Dot(_forwardDirectionNormalized, _hitDirectionNormalized);
        
        private float CalculateAngleBetweenDirections() => Vector3.Angle(_forwardDirectionNormalized, _hitDirectionNormalized);
        
        private Vector3 CalculateCrossProduct() => Vector3.Cross(_forwardDirectionNormalized, _hitDirectionNormalized);
    }
}