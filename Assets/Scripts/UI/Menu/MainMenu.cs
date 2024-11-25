namespace RehvidGames.UI.Menu
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class MainMenu : MonoBehaviour
    {
        #region Events
        public void OnPlayGame()
        {
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