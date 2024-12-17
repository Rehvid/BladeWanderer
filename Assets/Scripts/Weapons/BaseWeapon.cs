namespace RehvidGames.Weapons
{
    using System;
    using Interfaces;
    using UnityEngine;

    public abstract class BaseWeapon : MonoBehaviour, IWeapon
    {
        [SerializeField] protected Collider damageCollider;
        [SerializeField] protected WeaponStats weaponStats;
        [SerializeField] protected string id;
        [SerializeField] protected string name;
        
        public WeaponStats Stats => weaponStats;
        
        public bool IsCurrentlyEquipped { get; private set; }

        public string Id => id; 
        public string Name => name;

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(id))
            {
                id = Guid.NewGuid().ToString();
            }

            if (string.IsNullOrEmpty(name))
            {
                name = gameObject.name;
            }
        }
        
        public void RegenerateId() => id = Guid.NewGuid().ToString();

        public virtual void EnableDamageCollider()
        {
            damageCollider.enabled = true;
            damageCollider.isTrigger = true;
        }

        public virtual void DisableDamageCollider()
        {
            damageCollider.enabled = false;
            damageCollider.isTrigger = false;
        }

        
        #region Events
        public void OnEnableDamageCollider() => EnableDamageCollider();

        public void OnDisableDamageCollider() => DisableDamageCollider();
        #endregion
        
        public void Equip(Transform socket)
        {
            EquipToSocket(socket);
            SetCurrentlyEquipped(true);
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
            SetCurrentlyEquipped(false);
        }
        
        public void SetCurrentlyEquipped(bool equipped)
        {
            IsCurrentlyEquipped = equipped;
        }
        
        protected void OnTriggerEnter(Collider otherCollider)
        {
            if (!IsCurrentlyEquipped) return;
            
            otherCollider.TryGetComponent(out IDamageable target);
            if (CanTargetReceiveDamage(target))
            {
                ApplyDamage(target,  otherCollider.ClosestPoint(transform.position));
            }
        }
        
        protected virtual void ApplyDamage(IDamageable target, Vector3 hitPosition)
        {
            target.ReceiveDamage(Stats.Damage, hitPosition);
        }

        protected virtual bool CanTargetReceiveDamage(IDamageable target)
        {
            return target != null && !target.IsDead();
        }
    }
}
