namespace RehvidGames.UI.Menu
{
    using System.Collections.Generic;
    using DataPersistence.Data.State;
    using DataPersistence.Managers;
    using TMPro;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class SaveMenu : MonoBehaviour
    {
        [Header("Menu")] 
        [SerializeField] private TextMeshProUGUI _header;
        
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
            UpdateHeader();
            
            Dictionary<string, GameState> profilesGameData = GameStatePersistenceManager.Instance.GetAllProfiles();
            foreach (var saveSlot in _saveSlots)
            {
                ActiveSlot(profilesGameData, saveSlot);
            }
        }

        private void UpdateHeader()
        {
            if (_isLoadingGame)
            {
                _header.text = "Wczytaj grę";
                return;
            }

            _header.text = "Wybierz slot";
        }

        private void ActiveSlot(Dictionary<string, GameState> profilesGameData, SaveSlot saveSlot)
        {
            profilesGameData.TryGetValue(saveSlot.ProfileId, out var gameState);
            saveSlot.SetData(gameState);
            if (gameState == null && _isLoadingGame)
            {
                saveSlot.SetInteractable(false);
            }
            else
            {
                saveSlot.SetInteractable(true);
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
            GameStatePersistenceManager.Instance.ChangeSelectedProfileId(saveSlot.ProfileId);
            GameStatePersistenceManager.Instance.LoadData();
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
            GameStatePersistenceManager.Instance.ChangeSelectedProfileId(saveSlot.ProfileId);
            GameStatePersistenceManager.Instance.NewGame();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            GameStatePersistenceManager.Instance.SaveData();
        }
        
        public void OnClearClicked(SaveSlot saveSlotMenuItem)
        {
            _confirmationPopupMenu.ActivateMenu(
                _clearSaveSlotTitle,
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