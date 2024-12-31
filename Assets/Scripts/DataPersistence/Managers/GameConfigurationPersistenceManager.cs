namespace RehvidGames.DataPersistence.Managers
{
    using BackupServices;
    using Base;
    using Data.Configuration;
    using DataHandlers;
    using Interfaces;
    using RehvidGames.Managers;
    using Serializers;
    using Service;
    using UnityEngine;

    public class GameConfigurationPersistenceManager: BaseDataPersistence<GameConfiguration>
    {
        public static GameConfigurationPersistenceManager Instance { get; private set; }
        
        private void Awake()
        {
            InitializeInstance();
            InitializePersistenceObjects();
            InstanceComponents();
        }

        private void InitializeInstance()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void InitializePersistenceObjects()
        {
            persistenceObjects = RegistryManager<IDataPersistence<GameConfiguration>>.Instance.RegisteredObjects;
        }
        
        private void InstanceComponents()
        {
            dataManager = new DataManager<GameConfiguration>();
            dataHandler = new FileDataHandler(
                Application.persistentDataPath, 
                fileName, 
                new FileSerializer(useEncryption),
                new FileBackupService()
            );
            persistenceService = new GameConfigurationPersistenceService(dataHandler, dataManager);
        }

        private void Start()
        {
            LoadData();
        }

        public override void SaveData() => persistenceService.Save(persistenceObjects);

        public override void LoadData() => persistenceService.Load(persistenceObjects);
        
        private void OnApplicationQuit()
        {
            SaveData();
        }
    }
}