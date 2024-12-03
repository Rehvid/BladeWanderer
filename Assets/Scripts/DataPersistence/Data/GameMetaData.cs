namespace RehvidGames.DataPersistence.Data
{
    using System;
    using UI.Menu;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    [Serializable]
    public class GameMetaData
    {
        public string CurrentSceneName;
        public string ProfileId;
        public string LastUpdated;

        public void UpdateLastUpdated()
        {
            LastUpdated = DateTime.UtcNow.ToString("o");
        }

        public void UpdateCurrentSceneName()
        {
            string currentActiveSceneName = SceneManager.GetActiveScene().name;
            if (currentActiveSceneName != MainMenu.MainMenuSceneName)
            {
                CurrentSceneName = currentActiveSceneName; 
                return;
            }
            
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            CurrentSceneName = nextSceneIndex < SceneManager.sceneCountInBuildSettings
                ? SceneManager.GetSceneAt(nextSceneIndex).name
                : currentActiveSceneName;
        }
        
        public string GetFormattedLastUpdated(string format = "dd.MM.yyyy HH:mm:ss")
        {
            DateTime parsedDate = DateTime.Parse(LastUpdated).ToLocalTime();
            return parsedDate.ToString(format);
        }

        public DateTime GetDateTimeFromLastUpdated()
        {
            return  DateTime.Parse(LastUpdated);
        }
    }
}