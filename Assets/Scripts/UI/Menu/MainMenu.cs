namespace RehvidGames.UI.Menu
{
    using System;
    using DataPersistence;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class MainMenu : MonoBehaviour
    {
        [Header("Menu Buttons")]
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _continueGameButton;
        [SerializeField] private Button _loadGameButton;
        
        [Header("Menu sections")]
        [SerializeField] private SaveSlotMenu _saveSlotMenu;

        private void Start()
        {
            DisableMainMenuButtonsDependingOfData();
        }

        public void DisableMainMenuButtonsDependingOfData()
        {
            if (DataPersistenceManager.Instance.GetAllProfilesGameData().Count <= 0)
            {
                _continueGameButton.interactable = false;
                _loadGameButton.interactable = false;
            }
        }

        #region Events
        public void OnPlayGame()
        {
            _saveSlotMenu.ActivateMenu(false);
        }

        public void OnLoadGame()
        {
            _saveSlotMenu.ActivateMenu(true); 
        }

        public void OnContinueGame()
        {
            DataPersistenceManager.Instance.SaveGame();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void OnExitGame()
        {
            Debug.Log("Quit");
            Application.Quit();
        }
        #endregion
    }
}