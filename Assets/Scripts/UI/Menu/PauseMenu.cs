namespace RehvidGames.UI.Menu
{
    using Managers;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.SceneManagement;

    public class PauseMenu : MonoBehaviour
    {
        [Header("Menu sections")]
        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private SaveMenu saveMenu;
        
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
        
        public void OnMainMenu()
        {
            GameManager.Instance.LoadMainMenu();
            SceneManager.LoadScene(MainMenu.SceneName);  
        }

        public void OnLoadSlot()
        {
            saveMenu.FindSaveSlots();
            saveMenu.ActivateMenu(true); 
        }

        public void OnSaveSlot()
        {
            saveMenu.FindSaveSlots();
            saveMenu.ActivateMenu(false); 
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