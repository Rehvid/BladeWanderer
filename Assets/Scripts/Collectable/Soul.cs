namespace RehvidGames.Collectable
{
    using Player;
    using UnityEngine;
    
    public class Soul: Collectable
    {
        [SerializeField] private int _amount = 5;
        
        public override void Collect(Player player)
        {
            player.Attributes.AddSouls(_amount);
            Destroy(gameObject);
        }
    }
}