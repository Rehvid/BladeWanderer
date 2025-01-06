namespace RehvidGames.UI.Menu
{
    using DataPersistence.Managers;
    using Handler;
    using Managers;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class DeathMenu : MonoBehaviour
    {
        [Header("Menu sections")]
        [SerializeField] private GameObject _deathMenu;
        [SerializeField] private SaveMenu _saveMenu;
        
        [Header("Handler")]
        [SerializeField] private MainMenuHandler _mainMenuHandler;
        
        
        public void OnLoadSlot()
        {
            _saveMenu.FindSaveSlots();
            _saveMenu.ActivateMenu(true); 
        }

        public void OnContinueGame()
        {
            GameStatePersistenceManager.Instance.LoadMostRecentlyUpdatedProfileId();
        }
    }
}