namespace RehvidGames.Items.Weapons.Base
{
    using RehvidGames.Audio;
    using RehvidGames.Interfaces;
    using RehvidGames.Items.Base;
    using RehvidGames.Items.Weapons;
    using RehvidGames.VFX;
    using UnityEngine;

    public abstract class BaseWeapon : BaseItem
    { 
        [Header("Effects")]
        [SerializeField] protected WeaponEffectData effectData;
        
        [Header("Stats")]
        [SerializeField] protected WeaponStatsData weaponStatsData;
        
        [Header("Collider")]
        [SerializeField] protected Collider damageCollider;
        
        public WeaponStatsData StatsData => weaponStatsData;
        
        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
            damageCollider.isTrigger = true;
        }

        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
            damageCollider.isTrigger = false;
        }
        
        public void PlayEffects()
        {
            if (effectData == null) return;
            
            VFXManager.Instance.PlayVisualEffect(effectData.VisualEffect, transform.position);
            AudioManager.Instance.PlayClip(effectData.AudioData.SoundType, effectData.AudioData.SoundName);
        }
        
        public void Equip(Transform socket)
        {
            EquipToSocket(socket);
        }
        
        private void EquipToSocket(Transform socket)
        {
            transform.position = socket.position;
            transform.rotation = socket.rotation;
            transform.SetParent(socket);
        }

        public void UnEquip(Transform socket)
        {
            DisableDamageCollider();
            EquipToSocket(socket);
        }
        
        protected virtual void OnTriggerEnter(Collider otherCollider)
        {
            otherCollider.TryGetComponent(out IDamageable target);
            if (CanTargetReceiveDamage(target))
            {
                ApplyDamage(target,otherCollider.ClosestPoint(transform.position));
            }
        }
        
        protected void ApplyDamage(IDamageable target, Vector3 hitPosition)
        {
            target.ReceiveDamage(StatsData.BaseDamage, hitPosition);
        }
        
        protected bool CanTargetReceiveDamage(IDamageable target) => target != null && !target.IsDead();
    }
}
