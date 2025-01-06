namespace RehvidGames.VFX
{
    using Enums;
    using UnityEngine;
    using Utilities;

    public class VFXManager: BaseSingletonMonoBehaviour<VFXManager>
    {
        public void PlayParticleEffect(VFXConfig config, Vector3 position)
        {
            GameObject effectInstance = CreateEffectInstance(config, position, VFXType.ParticleEffect);
            
            if (effectInstance?.TryGetComponent(out ParticleSystem particle) != true) return;
            
            particle.Play();
            Destroy(effectInstance, config.Duration);
        }

        public void PlayVisualEffect(VFXConfig config, Vector3 position)
        {
            GameObject effectInstance = CreateEffectInstance(config, position, VFXType.VisualEffect);
            
            if (effectInstance == null) return;
            
            effectInstance.SetActive(true);
            Destroy(effectInstance, config.Duration);
        }

        private GameObject CreateEffectInstance(VFXConfig config, Vector3 position, VFXType type) => 
            IsValidVfxConfig(config, type) ? Instantiate(config.EffectPrefab, position, Quaternion.identity) : null;
        
        
        private bool IsValidVfxConfig(VFXConfig config, VFXType expectedType) 
            => config.EffectPrefab != null && config.VfxType == expectedType;
    }
}