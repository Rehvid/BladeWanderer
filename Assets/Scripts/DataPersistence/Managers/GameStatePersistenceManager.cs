namespace RehvidGames.DataPersistence.Managers
{
    using System.Collections.Generic;
    using BackupServices;
    using Base;
    using Data.State;
    using DataHandlers;
    using Interfaces;
    using RehvidGames.Managers;
    using UI.Menu;
    using Serializers;
    using Service;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class GameStatePersistenceManager: BaseDataPersistence<GameState>
    {
        public static GameStatePersistenceManager Instance { get; private set; }
        
        [Header("Debugging")]
        [SerializeField] private bool _initializeDataIfNull;
        
        [Header("Auto Saving Configuration")] 
        [SerializeField] private AutoSaveManager autoSaveManager;
        
        private ProfileManager _profileManager;
        
        #region Initialization
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            persistenceObjects = RegistryManager<IDataPersistence<GameState>>.Instance.RegisteredObjects;
            InitializeInstance();
            InitializeComponents();
            persistenceService.Load(persistenceObjects);
        }

        private void InitializeInstance()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void InitializeComponents()
        {
            dataHandler = new FileDataHandler(
                Application.persistentDataPath,
                fileName, 
                new FileSerializer(useEncryption),
                new FileBackupService()
            );
            _profileManager = new ProfileManager(dataHandler);
            dataManager = new DataManager<GameState>();
            persistenceService = new GameStatePersistenceService(
                dataHandler, 
                dataManager, 
                _profileManager,
                _initializeDataIfNull
            );

            if (autoSaveManager !=null)
            { 
                autoSaveManager.SetGameStatePersistenceService(persistenceService);
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
            RegistryManager<IDataPersistence<GameState>>.Instance.RegisterMonoBehaviours(); 
            persistenceObjects = RegistryManager<IDataPersistence<GameState>>.Instance.RegisteredObjects;
            if (scene.name == MainMenu.SceneName) return;
            
            LoadData();
            autoSaveManager.HandleAutoSaveCoroutine();
        }
        #endregion

        public Dictionary<string, GameState> GetAllProfiles() => dataHandler.LoadAllProfiles();
        
        public bool HasAnyProfiles() => GetAllProfiles().Count > 0;
        
        public void NewGame() => dataManager.ResetData();
        
        public override void LoadData() => persistenceService.Load(persistenceObjects);

        public void LoadMostRecentlyUpdatedProfileId()
        {
            GameState recentlyUpdatedState = dataHandler.LoadGameState(dataHandler.GetMostRecentlyUpdatedProfileId());
            if (recentlyUpdatedState != null)
            {
                SceneManager.LoadScene(recentlyUpdatedState.SessionData.CurrentSceneName);
            }
        }
        
        public override void SaveData() => persistenceService.Save(persistenceObjects);
        
        public void ChangeSelectedProfileId(string newProfileId)
        {
            _profileManager.SetCurrentProfile(newProfileId);
            LoadData();
        }

        public void DeleteProfileData(string profileId)
        {
            _profileManager.DeleteProfile(profileId);
            LoadData();
        }
        
        
        private void OnApplicationQuit()
        {
            SaveData();
        }
    }
}