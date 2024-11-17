namespace RehvidGames.Collectable
{
    using Player;
    
    public class Soul: Treasure
    {
        public override void Collect(Player player)
        {
            player.Attributes.AddSouls(GetTreasureAmount());
            Destroy(gameObject);
        }
    }
}