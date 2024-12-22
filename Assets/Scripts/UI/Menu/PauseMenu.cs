namespace RehvidGames.UI.Menu
{
    using Managers;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.SceneManagement;

    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _pauseMenu;
        
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
        
        public void OnPause(InputAction.CallbackContext context)
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