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
            string currentSceneName = SceneManager.GetActiveScene().name;
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                CurrentSceneName = currentSceneName == MainMenu.MainMenuSceneName 
                    ? SceneManager.GetSceneByBuildIndex(nextSceneIndex).name 
                    : currentSceneName;
            }
            else
            {
                //TODO: Tutaj coś nie działa
                
                // Obsłuż przypadek, gdy nie ma kolejnej sceny
                Debug.LogWarning("There is no next scene in Build Settings!");
                CurrentSceneName = currentSceneName; 
            }
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