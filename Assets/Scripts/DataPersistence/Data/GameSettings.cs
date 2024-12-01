namespace RehvidGames.DataPersistence.Data
{
    using System;

    [Serializable]
    public class GameSettings
    {
        public GameGameplaySettings GameplaySettings = new();
        public GameAudioSettings GameAudioSettings = new();
    }
}