namespace RehvidGames.UI.Menu
{
    using Handler;
    using Managers;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.SceneManagement;

    public class PauseMenu : MonoBehaviour
    {
        [Header("Menu sections")]
        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private SaveMenu _saveMenu;
        
        [Header("Handler")]
        [SerializeField] private MainMenuHandler _mainMenuHandler;
        
        public void OnResume()
        {
            if (!GameManager.Instance.IsPaused) return;
            ResumeGame();
        }

        private void ResumeGame()
        {
            _pauseMenu.SetActive(false);
            GameManager.Instance.ResumeGame();
        }
        
        public void OnLoadSlot()
        { 
            HandleSaveMenu(true);
        }

        public void OnSaveSlot()
        {
            HandleSaveMenu(false);
        }

        private void HandleSaveMenu(bool isLoadingGame)
        {
            _saveMenu?.FindSaveSlots();
            _saveMenu?.ActivateMenu(isLoadingGame); 
        }
        
        public void OnPausePerformed(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            
            if (GameManager.Instance.IsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        private void PauseGame()
        {
            GameManager.Instance.PauseGame();
            _pauseMenu.SetActive(true);
        }
    }
}