namespace RehvidGames.VFX
{
    using UnityEngine;
    using UnityEngine.VFX;
    using Utilities;

    public class VFXManager: BaseSingletonMonoBehaviour<VFXManager>
    {
        public void PlayParticleEffect(VFXConfig config, Vector3 position)
        {
            if (!IsValidVfxConfig(config, VFXType.ParticleEffect)) return;
            
            GameObject effectInstance = Instantiate(config.EffectPrefab, position, Quaternion.identity);
            
            if (!effectInstance.TryGetComponent(out ParticleSystem particle)) return;
            
            particle.Play();
            Destroy(effectInstance, config.Duration);
        }

        public void PlayVisualEffect(VFXConfig config, Vector3 position)
        {
            if (!IsValidVfxConfig(config, VFXType.VisualEffect)) return;
            
            GameObject effectInstance = Instantiate(config.EffectPrefab, position, Quaternion.identity);
            
            if (!effectInstance.TryGetComponent(out VisualEffect visualEffect)) return;
            
            effectInstance.SetActive(true);
            Destroy(effectInstance, config.Duration);
        }
        private bool IsValidVfxConfig(VFXConfig config, VFXType expectedType) 
            => config.EffectPrefab != null && config.VfxType == expectedType;
    }
}