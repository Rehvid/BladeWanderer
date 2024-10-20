namespace RehvidGames.Collectable
{
    using Interfaces;
    using Player;
    using UnityEngine;

    public abstract class Collectable: MonoBehaviour, ICollectable
    {
        public void OnTriggerEnter(Collider otherCollider)
        {
            Collect(otherCollider.GetComponent<Player>());
        }
        
        public abstract void Collect(Player player);
    }
}