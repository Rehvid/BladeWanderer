namespace RehvidGames.DataPersistence.Managers
{
    using System.Collections.Generic;
    using RehvidGames.DataPersistence.Data;
    using RehvidGames.DataPersistence.DataHandlers;
    using RehvidGames.Interfaces;
    using RehvidGames.UI.Menu;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.Serialization;

    public class DataPersistenceManager: MonoBehaviour
    {
        public static DataPersistenceManager Instance { get; private set; }
        
        [Header("Debugging")]
        [SerializeField] private bool _initializeDataIfNull;
        
        [Header("File Storage Config")] 
        [SerializeField] private string _fileName;
        [SerializeField] private bool _useEncryption;
        
        [Header("Auto Saving Configuration")] 
        [SerializeField] private AutoSaveManager autoSaveManager;
        
        private List<IDataPersistence> _persistenceObjects;
        
        private IDataHandler _dataHandler;
        private ProfileManager _profileManager;
        private GameDataManager _gameDataManager;
        private SaveLoadManager _saveLoadManager;
        
        #region Initialization
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            InitializeInstance();
            InitializeComponents();
        }

        private void InitializeInstance()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void InitializeComponents()
        {
            _dataHandler = new FileDataHandler(Application.persistentDataPath, _fileName, _useEncryption);
            _profileManager = new ProfileManager(_dataHandler);
            _gameDataManager = new GameDataManager();
            _saveLoadManager = new SaveLoadManager(_dataHandler, _gameDataManager, _profileManager);

            if (autoSaveManager !=null)
            { 
                autoSaveManager.SetSaveLoadService(_saveLoadManager);
            }
        }
        
        #endregion
        
        #region Events
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        { 
            _persistenceObjects = DataPersistenceRegistryManager.Instance.GetAllRegisteredObjects();
            
            if (scene.name == MainMenu.MainMenuSceneName) return;
            
            LoadGame();
            autoSaveManager.HandleAutoSaveCoroutine();
        }
        #endregion

        public Dictionary<string, GameData> GetAllProfilesGameData()
        {
            return _dataHandler.LoadAllProfiles();
        }

        public bool HasAllProfilesGameData()
        {
            return GetAllProfilesGameData().Count > 0;
        }
        
        public void NewGame()
        {
            _gameDataManager.ResetGameData();
        }

        public void LoadGame()
        {
            _saveLoadManager.LoadGame(_initializeDataIfNull, _persistenceObjects);
        }
        
        public void SaveGame()
        {
            _saveLoadManager.SaveGame(_persistenceObjects);
        }
        
        public void ChangeSelectedProfileId(string newProfileId)
        {
            _profileManager.SetCurrentProfile(newProfileId);
            LoadGame();
        }

        public void DeleteProfileData(string profileId)
        {
            _profileManager.DeleteProfile(profileId);
            LoadGame();
        }
        
        private void OnApplicationQuit()
        {
            SaveGame();
        }
    }
}