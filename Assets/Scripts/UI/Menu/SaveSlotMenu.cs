namespace RehvidGames.UI.Menu
{
    using System.Collections.Generic;
    using DataPersistence;
    using Serializable;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class SaveSlotMenu : MonoBehaviour
    {
        [SerializeField] private ConfirmationPopupMenu _confirmationPopupMenu;
        
        private SaveSlotMenuItem[] _saveSlots;
        private bool _isLoadingGame;
        
        private void Awake()
        {
            _saveSlots = GetComponentsInChildren<SaveSlotMenuItem>();
        }
        
        public void ActivateMenu(bool isLoadingGame)
        {
            _isLoadingGame = isLoadingGame;
            
            Dictionary<string, GameData> profilesGameData = DataPersistenceManager.Instance.GetAllProfilesGameData();

            foreach (SaveSlotMenuItem saveSlotMenuItem in _saveSlots)
            {
                GameData profileData = null;
                profilesGameData.TryGetValue(saveSlotMenuItem.ProfileId, out profileData);
                saveSlotMenuItem.SetData(profileData);
                if (profileData == null && isLoadingGame)
                {
                    saveSlotMenuItem.SetInteractable(false);
                }
                else
                {
                    saveSlotMenuItem.SetInteractable(true);
                }
            }
        }

        public void OnSaveSlotClicked(SaveSlotMenuItem saveSlotMenuItem)
        {
            
            if (_isLoadingGame)
            {
                DataPersistenceManager.Instance.ChangeSelectedProfileId(saveSlotMenuItem.ProfileId);
                SaveGameAndLoadScene();
            } else if (saveSlotMenuItem.HasData)
            {
                _confirmationPopupMenu.ActivateMenu(
                    "Starting a New Game with this slot will overwrite any existing profile data. Are you sure you want to continue?",
                    () =>
                    {
                        DataPersistenceManager.Instance.ChangeSelectedProfileId(saveSlotMenuItem.ProfileId);
                        DataPersistenceManager.Instance.NewGame();
                        SaveGameAndLoadScene();
                    },
                    () =>
                    {
                        
                    }
                );
            }
            else
            {
                DataPersistenceManager.Instance.ChangeSelectedProfileId(saveSlotMenuItem.ProfileId);
                DataPersistenceManager.Instance.NewGame();
                SaveGameAndLoadScene();
            }
        }

        private void SaveGameAndLoadScene()
        {
            DataPersistenceManager.Instance.SaveGame();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void OnClearClicked(SaveSlotMenuItem saveSlotMenuItem)
        {
            _confirmationPopupMenu.ActivateMenu(
                "Are you sure you want to clear this saved data?",
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