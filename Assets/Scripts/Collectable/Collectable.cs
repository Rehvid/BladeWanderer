namespace RehvidGames.Collectable
{
    using Interfaces;
    using Player;
    using UnityEngine;

    public abstract class Collectable: MonoBehaviour, ICollectable
    {
        public void OnTriggerEnter(Collider otherCollider)
        {
            if (otherCollider.TryGetComponent(out Player player))
            {
                Collect(player);
            }
        }
        
        public abstract void Collect(Player player);
    }
}