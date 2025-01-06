namespace RehvidGames.UI.Menu
{
    using DataPersistence.Managers;
    using Managers;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class DeathMenu : MonoBehaviour
    {
        [Header("Menu sections")]
        [SerializeField] private GameObject _deathMenu;
        [SerializeField] private SaveMenu saveMenu;
        
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

        public void OnContinueGame()
        {
            GameStatePersistenceManager.Instance.LoadMostRecentlyUpdatedProfileId();
        }
    }
}