namespace RehvidGames.Items.Weapons.Interfaces
{
    using Characters.Player;
    using RehvidGames.Interfaces;
    using UnityEngine;

    public interface IPickUpWeapon: IInteractable
    {
        public void PickUp(PlayerController player);
        public Collider GetPickableWeaponCollider();
    }
}