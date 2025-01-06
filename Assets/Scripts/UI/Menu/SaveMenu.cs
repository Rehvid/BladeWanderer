namespace RehvidGames.UI.Menu
{
    using System.Collections.Generic;
    using Data;
    using DataPersistence.Data.State;
    using DataPersistence.Managers;
    using DG.Tweening;
    using Managers;
    using TMPro;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.SceneManagement;

    public class SaveMenu : MonoBehaviour
    {
        [Header("Menu")] 
        [SerializeField] private TextMeshProUGUI _header;
        [SerializeField] private SaveMenuTitleData saveMenuTitleData;
        
        [Header("Confirmation popup menu")]
        [SerializeField] private ConfirmationPopupMenu _confirmationPopupMenu;
        
        private SaveSlot[] _saveSlots;
        private bool _isLoadingGame;

        private void Awake() => FindSaveSlots();

        public void FindSaveSlots()
        {
            _saveSlots ??= GetComponentsInChildren<SaveSlot>();
        }
        
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
            if (GameManager.Instance.IsPaused)
            {
                saveSlot.FindSaveSlotButton(); 
            }
            
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
            else if (GameManager.Instance.IsPaused)
            {
                SaveGameOnSaveSlotClicked(saveSlot);
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
            if (GameManager.Instance.IsPaused)
            {
                DOTween.KillAll();
                GameManager.Instance.ResumeGame();
            }
            
            SceneManager.LoadScene(saveSlot.IndexScene, LoadSceneMode.Single);
        }

        private void OverrideSaveSlotClicked(SaveSlot saveSlot)
        {
            _confirmationPopupMenu.ActivateMenu(
                saveMenuTitleData.OverrideSlotTitle,
                () =>
                {
                    if (GameManager.Instance.IsPaused)
                    {
                        SaveGameOnSaveSlotClicked(saveSlot);
                        ActivateMenu(_isLoadingGame); 
                        return;
                    }
                    
                    StartNewGameOnSaveSlotClicked(saveSlot);
                },
                () => {  ActivateMenu(_isLoadingGame); }
            );
        }

        private void StartNewGameOnSaveSlotClicked(SaveSlot saveSlot)
        {
            GameStatePersistenceManager.Instance.ChangeSelectedProfileId(saveSlot.ProfileId);
            GameStatePersistenceManager.Instance.NewGame();
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
            
            GameStatePersistenceManager.Instance.IsNewGame = true;
        }

        private void SaveGameOnSaveSlotClicked(SaveSlot saveSlot)
        {
            GameStatePersistenceManager.Instance.ChangeSelectedProfileId(saveSlot.ProfileId);
            GameStatePersistenceManager.Instance.SaveData();
            
            ActivateMenu(_isLoadingGame); 
            EventSystem.current.SetSelectedGameObject(null);
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