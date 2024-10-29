namespace RehvidGames.Interfaces
{
    using UnityEngine;

    public interface IHealth
    {
        public void ReceiveDamage(float damage, Vector3 hitPosition);
        public bool IsDead();
    }
}