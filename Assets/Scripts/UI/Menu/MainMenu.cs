namespace RehvidGames.UI.Menu
{
    using System;
    using DataPersistence;
    using DataPersistence.Managers;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class MainMenu : MonoBehaviour
    {
        public const string MainMenuSceneName = "Menu";
        
        [Header("Menu Buttons")]
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _continueGameButton;
        [SerializeField] private Button _loadGameButton;
        
        [Header("Menu sections")]
        [SerializeField] private SaveMenu saveMenu;
        
        private void Start()
        {
            DisableMainMenuButtonsDependingOfData();
        }

        private void DisableMainMenuButtonsDependingOfData()
        {
            if (DataPersistenceManager.Instance.HasAllProfilesGameData()) return;
            
            _continueGameButton.interactable = false;
            _loadGameButton.interactable = false;
        }

        #region Events

        private void OnEnable()
        {
            DisableMainMenuButtonsDependingOfData();
        }

        public void OnPlayGameClicked()
        {
            saveMenu.ActivateMenu(false);
        }

        public void OnLoadGameClicked()
        {
            saveMenu.ActivateMenu(true); 
        }

        public void OnContinueGameClicked()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void OnExitGameClicked()
        {
            Debug.Log("Quit");
            Application.Quit();
        }
        #endregion
    }
}