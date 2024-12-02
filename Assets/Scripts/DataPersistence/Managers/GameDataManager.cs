namespace RehvidGames.DataPersistence.Managers
{
    using RehvidGames.DataPersistence.Data;

    public class GameDataManager
    {
        private GameData _gameData = new(); 

        public GameData GetGameData()
        {
            return _gameData;
        }

        public void SetGameData(GameData gameData)
        {
            _gameData = gameData;
        }

        public void ResetGameData()
        {
            _gameData = new GameData();
        }

        public bool HasGameData()
        {
            return _gameData != null;
        }
    }
}