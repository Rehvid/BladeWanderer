namespace RehvidGames.UI.Menu
{
    using System.Collections.Generic;
    using DataPersistence.Data.State;
    using DataPersistence.Managers;
    using ScriptableObjects;
    using TMPro;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.Serialization;

    public class SaveMenu : MonoBehaviour
    {
        [Header("Menu")] 
        [SerializeField] private TextMeshProUGUI _header;
        [FormerlySerializedAs("_saveMenuTitle")] [SerializeField] private SaveMenuTitleData saveMenuTitleData;
        
        [Header("Confirmation popup menu")]
        [SerializeField] private ConfirmationPopupMenu _confirmationPopupMenu;
        
        private SaveSlot[] _saveSlots;
        private bool _isLoadingGame;
        
        private void Awake() => _saveSlots = GetComponentsInChildren<SaveSlot>();
        
        public void ActivateMenu(bool isLoadingGame)
        {
            _isLoadingGame = isLoadingGame;
            UpdateHeaderText();
            ActivateAllSlots();
        }

        private void UpdateHeaderText()
        {
            _header.text = _isLoadingGame ? saveMenuTitleData.HeaderLoadingSlotTitle : saveMenuTitleData.HeaderSlotTitle;
        }

        private void ActivateAllSlots()
        {
            Dictionary<string, GameState> profiles = GameStatePersistenceManager.Instance.GetAllProfiles();
            foreach (var saveSlot in _saveSlots)
            {
                ActiveSlot(profiles, saveSlot);
            }
        }

        private void ActiveSlot(Dictionary<string, GameState> profiles, SaveSlot saveSlot)
        {
            profiles.TryGetValue(saveSlot.ProfileId, out var gameState);
            saveSlot.SetData(gameState);
            if (gameState == null && _isLoadingGame)
            {
                saveSlot.SetInteractable(false);
                return;
            }
            
            saveSlot.SetInteractable(true);
        }
        
        public void OnSaveSlotClicked(SaveSlot saveSlot)
        {
            if (_isLoadingGame)
            {
                LoadSaveSlotClicked(saveSlot);
            } else if (saveSlot.IsDataAvailable)
            {
                OverrideSaveSlotClicked(saveSlot);
            }
            else
            {
                StartNewGameOnSaveSlotClicked(saveSlot);
            }
        }

        private void LoadSaveSlotClicked(SaveSlot saveSlot)
        {
            GameStatePersistenceManager.Instance.ChangeSelectedProfileId(saveSlot.ProfileId);
            GameStatePersistenceManager.Instance.LoadData();
            SceneManager.LoadScene(saveSlot.CurrentSceneName);
        }

        private void OverrideSaveSlotClicked(SaveSlot saveSlot)
        {
            _confirmationPopupMenu.ActivateMenu(
                saveMenuTitleData.OverrideSlotTitle,
                () => { StartNewGameOnSaveSlotClicked(saveSlot); },
                () => { }
            );
        }

        private void StartNewGameOnSaveSlotClicked(SaveSlot saveSlot)
        {
            GameStatePersistenceManager.Instance.ChangeSelectedProfileId(saveSlot.ProfileId);
            GameStatePersistenceManager.Instance.NewGame();
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            
            GameStatePersistenceManager.Instance.SaveData();
        }
        
        public void OnClearClicked(SaveSlot saveSlotMenuItem)
        {
            _confirmationPopupMenu.ActivateMenu(
                saveMenuTitleData.ClearSlotTitle,
                () => 
                {
                    GameStatePersistenceManager.Instance.DeleteProfileData(saveSlotMenuItem.ProfileId);
                    ActivateMenu(_isLoadingGame);
                },
                () =>
                {
                    ActivateMenu(_isLoadingGame);
                }
            );
        }
    }
}