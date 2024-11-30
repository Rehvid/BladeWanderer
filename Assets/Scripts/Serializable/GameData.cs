namespace RehvidGames.Serializable
{
    using UnityEngine.SceneManagement;

    [System.Serializable]
    public class GameData
    {
        public string CurrentSceneName;
        public long LastUpdated;
        public PlayerGameData PlayerGameData = new();
    }
}