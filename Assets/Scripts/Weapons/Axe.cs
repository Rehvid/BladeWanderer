namespace RehvidGames.Weapons
{
    using Player;
    
    public class Axe : BaseWeapon
    {
        
        public override void EnableDamageCollider()
        {
            damageCollider.enabled = true;
            damageCollider.isTrigger = true;
        }

        public override void DisableDamageCollider()
        {
            damageCollider.enabled = false;
            damageCollider.isTrigger = false;
        }
        public override void Interact(Player player)
        { }

        public override bool CanInteract(Player player)
        { 
            return true;
        }
    }
}