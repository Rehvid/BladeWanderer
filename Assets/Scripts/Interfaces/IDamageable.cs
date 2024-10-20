namespace RehvidGames.Interfaces
{
    using UnityEngine;

    public interface IDamageable
    {
        public void ReceiveDamage(float damage, Vector3 hitPosition);
        public bool IsDead();
    }
}