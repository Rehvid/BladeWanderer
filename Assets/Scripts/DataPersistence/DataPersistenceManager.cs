namespace RehvidGames.DataPersistence
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;
    using Serializable;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    
    public class DataPersistenceManager: MonoBehaviour
    {
        [Header("Debugging")]
        [SerializeField] private bool _initializeDataIfNull;
        [SerializeField] private bool overrideSelectedProfileId;
        
        [Header("File Storage Config")] 
        [SerializeField] private string _fileName;
        [SerializeField] private bool _useEncryption;

        [Header("Auto Saving Configuration")] 
        [SerializeField] private float _autoSaveTimeSeconds = 60f;
        
        public static DataPersistenceManager Instance { get; private set; }
        
        private GameData _gameData;
        private List<IDataPersistence> _persistenceObjects;
        private FileDataHandler _dataHandler;
        public string _selectedProfileId = "";

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _dataHandler = new FileDataHandler(Application.persistentDataPath, _fileName, _useEncryption);
            InitializeSelectedProfileId();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        
        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _persistenceObjects = FindAllPersistenceObjects();
            LoadGame();
            Coroutine autoSaveCoroutine = null;
            if (autoSaveCoroutine != null)
            {
                StopCoroutine(autoSaveCoroutine);
            }
            autoSaveCoroutine = StartCoroutine(AutoSave());
        }

        
        public void NewGame()
        {
            _gameData = new GameData();
        }

        public void LoadGame()
        {
            _gameData = _dataHandler.Load(_selectedProfileId); 

            if (_gameData == null && _initializeDataIfNull)
            {
                NewGame();
            }
            
            if (_gameData == null)
            {
                Debug.Log("No data was found. Initializing new game...");
                return;
            }

            foreach (IDataPersistence persistenceObject in _persistenceObjects)
            {
                persistenceObject.LoadData(_gameData);
            }
        }

        public void SaveGame()
        {
            if (_gameData == null) return;
            
            foreach (IDataPersistence persistenceObject in _persistenceObjects)
            {
                persistenceObject.SaveData(_gameData);
            }
            
            _gameData.LastUpdated = System.DateTime.Now.ToBinary();
            
            _dataHandler.Save(_gameData, _selectedProfileId);
        }
        
        public bool HasGameData() => _gameData != null;

        public void ChangeSelectedProfileId(string newProfileId)
        {
            _selectedProfileId = newProfileId;
            LoadGame();
        }

        public void DeleteProfileData(string profileId)
        {
            _dataHandler.Delete(profileId);
            InitializeSelectedProfileId();
            LoadGame();
        }

        private void InitializeSelectedProfileId()
        {
            _selectedProfileId = _dataHandler.GetMostRecentlyUpdatedProfileId();
        }
        
        public Dictionary<string, GameData> GetAllProfilesGameData() => _dataHandler.LoadAllProfiles();
        
        private void OnApplicationQuit()
        {
            SaveGame();
        }

        private List<IDataPersistence> FindAllPersistenceObjects()
        {
            //TODO: Problem with inavtice objects (check it)
            return FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .OfType<IDataPersistence>()
                .ToList();
        }

        private IEnumerator AutoSave()
        {
            while (true)
            {
                yield return new WaitForSeconds(_autoSaveTimeSeconds);
                SaveGame();
                Debug.Log("Auto saving...");
            }
        }
    }
}