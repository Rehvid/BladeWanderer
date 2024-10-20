namespace RehvidGames.Weapons
{
    using Interfaces;
    using Player;
    using UnityEngine;

    public abstract class BaseWeapon : MonoBehaviour, IInteractable, IWeapon
    {
        [SerializeField] protected Collider damageCollider;
        [SerializeField] protected WeaponStats weaponStats;
        
        public WeaponStats Stats => weaponStats;
        
        public bool IsCurrentlyEquipped { get; private set; }

        public abstract void EnableDamageCollider();
        
        public abstract void DisableDamageCollider();
        
        public abstract void Interact(Player player);
        
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
