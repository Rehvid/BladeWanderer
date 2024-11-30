namespace RehvidGames.Interfaces
{
    using Serializable;

    public interface IDataHandler
    {
        public GameData Load(string profileId);
        public void Save(GameData data, string profileId);
    }
}