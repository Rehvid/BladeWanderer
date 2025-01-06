namespace RehvidGames.Collectable.Treasures
{
    using Audio;
    using Base;
    using Characters.Player;
    using Enums;

    public class Soul: BaseTreasure
    {
        public override void Collect(PlayerController player)
        {
            player.Attributes.AddSouls(GetTreasureAmount());
            
            AudioManager.Instance.PlayRandomClip(SoundType.ItemPickUpSoul);
            
            Destroy(gameObject);
        }
    }
}