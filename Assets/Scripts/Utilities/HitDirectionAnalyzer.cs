namespace RehvidGames.Utilities
{
    using System;
    using RehvidGames.Animator;
    using RehvidGames.Enums;
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
            
            if (CalculateDotProduct() <= DotProductThreshold)
            {
                Debug.Log("Uderzenie z tyłu");
                return HitDirectionType.Back;
            }

            return HitDirectionType.Front;
        }
        
        public HitDirectionType GetDirectionType(Vector3 hitPosition, Transform damageReceiver)
        {
            SetHitDirectionNormalized(hitPosition, damageReceiver);
            SetForwardDirectionNormalized(damageReceiver);

            if (CalculateDotProduct() <= DotProductThreshold)
            {
                Debug.Log("Uderzenie z tyłu");
                return HitDirectionType.Back;
            }
            
            if (CalculateAngleBetweenDirections() <= FrontHitAngleThreshold)
            {
                Debug.Log("Uderzenie z przodu");
                return HitDirectionType.Front;
            }
            
            if (CalculateCrossProduct().y > CrossProductThresholdY)
            {
                Debug.Log("Uderzenie z prawej strony");
                return HitDirectionType.Right;
            }
            
            Debug.Log("Uderzenie z lewej strony");
            return HitDirectionType.Left;
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