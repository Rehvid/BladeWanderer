namespace RehvidGames.Collectable
{
    using Audio;
    using Enums;
    using Player;

    public class Soul: Treasure
    {
        public override void Collect(Player player)
        {
            player.Attributes.AddSouls(GetTreasureAmount());
            AudioManager.Instance.PlayRandomClip(SoundType.ItemPickUpSoul);
            Destroy(gameObject);
        }
    }
}