namespace RehvidGames.Collectable.Treasures
{
    using Audio;
    using Base;
    using Characters.Player;
    using Enums;
    using Managers;
    using UnityEngine;

    public class Soul: BaseTreasure
    {
        public override void Collect(PlayerController player)
        {
            player.Attributes.AddSouls(GetTreasureAmount());
            
            AudioManager.Instance.PlayRandomClip(SoundType.ItemPickUpSoul);
            
            Destroy(gameObject);
            if (gameObject.name.Contains("Horseman"))
            {
                GameManager.Instance.GameFinished();
            }
        }
    }
}