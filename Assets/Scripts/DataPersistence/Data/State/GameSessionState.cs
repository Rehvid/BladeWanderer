namespace RehvidGames.DataPersistence.Data.State
{
    using System;
    using UI.Menu;
    using UnityEngine.SceneManagement;

    [Serializable]
    public class GameSessionState
    {
        public string CurrentSceneName;
        public int IndexScene;
        public string ProfileId;
        public string LastUpdated;

        public void UpdateLastUpdated()
        {
            LastUpdated = DateTime.UtcNow.ToString("o");
        }

        public void UpdateCurrentSceneName()
        {
            string currentActiveSceneName = SceneManager.GetActiveScene().name;
            if (currentActiveSceneName != MainMenu.SceneName)
            {
                 CurrentSceneName = currentActiveSceneName; 
            }
        }

        public void UpdateSceneIndex()
        {
            IndexScene = SceneManager.GetActiveScene().buildIndex;
        }
        
        public string GetFormattedLastUpdated(string format = "dd.MM.yyyy HH:mm:ss")
        {
            DateTime parsedDate = DateTime.Parse(LastUpdated).ToLocalTime();
            return parsedDate.ToString(format);
        }

        public DateTime GetDateTimeFromLastUpdated()
        {
            return DateTime.Parse(LastUpdated);
        }
    }
}