namespace RehvidGames.DataPersistence.Data.Configuration
{
    using System;
    using Serializers;

    [Serializable]
    public class GameConfiguration: ISavableData
    {
        public GameplaySettings GameplaySettings = new();
        public AudioSettings AudioSettings = new();
    }
}