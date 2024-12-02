namespace RehvidGames.UI.Menu
{
    using System.Collections.Generic;
    using DataPersistence;
    using DataPersistence.Data;
    using DataPersistence.Managers;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class SaveMenu : MonoBehaviour
    {
        [Header("Confirmation popup menu")]
        [SerializeField] private ConfirmationPopupMenu _confirmationPopupMenu;
        [SerializeField] private string _overrideSaveSlotTitle = "Rozpoczęcie nowej gry z tym slotem spowoduje nadpisanie istniejących danych profilu. Czy na pewno chcesz kontynuować?";
        [SerializeField] private string _clearSaveSlotTitle = "Czy na pewno chcesz wyczyścić zapisane dane?";
        
        private SaveSlot[] _saveSlots;
        private bool _isLoadingGame;
        
        private void Awake()
        {
            _saveSlots = GetComponentsInChildren<SaveSlot>();
        }
        
        public void ActivateMenu(bool isLoadingGame)
        {
            _isLoadingGame = isLoadingGame;
            
            Dictionary<string, GameData> profilesGameData = DataPersistenceManager.Instance.GetAllProfilesGameData();
            foreach (var saveSlot in _saveSlots)
            {
                profilesGameData.TryGetValue(saveSlot.ProfileId, out var profileData);
                saveSlot.SetData(profileData);
                if (profileData == null && isLoadingGame)
                {
                    saveSlot.SetInteractable(false);
                }
                else
                {
                    saveSlot.SetInteractable(true);
                }
            }
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
            DataPersistenceManager.Instance.ChangeSelectedProfileId(saveSlot.ProfileId);
            DataPersistenceManager.Instance.LoadGame();
            SceneManager.LoadScene(saveSlot.CurrentSceneName);
        }

        private void OverrideSaveSlotClicked(SaveSlot saveSlot)
        {
            _confirmationPopupMenu.ActivateMenu(
                _overrideSaveSlotTitle,
                () => { StartNewGameOnSaveSlotClicked(saveSlot); },
                () => { }
            );
        }

        private void StartNewGameOnSaveSlotClicked(SaveSlot saveSlot)
        {
            DataPersistenceManager.Instance.ChangeSelectedProfileId(saveSlot.ProfileId);
            DataPersistenceManager.Instance.NewGame();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            DataPersistenceManager.Instance.SaveGame();
        }
        
        public void OnClearClicked(SaveSlot saveSlotMenuItem)
        {
            _confirmationPopupMenu.ActivateMenu(
                _clearSaveSlotTitle,
                () => 
                {
                    DataPersistenceManager.Instance.DeleteProfileData(saveSlotMenuItem.ProfileId);
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