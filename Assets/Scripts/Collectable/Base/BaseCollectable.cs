namespace RehvidGames.Collectable.Base
{
    using Characters.Player;
    using Interfaces;
    using UnityEngine;

    public abstract class BaseCollectable: MonoBehaviour, ICollectable
    {
        public void OnTriggerEnter(Collider otherCollider)
        {
            if (otherCollider.TryGetComponent(out PlayerController player))
            {
                Collect(player);
            }
        }
        
        public abstract void Collect(PlayerController player);
    }
}