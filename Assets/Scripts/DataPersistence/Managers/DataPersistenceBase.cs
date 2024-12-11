namespace RehvidGames.DataPersistence.Managers
{
    using System.Collections.Generic;
    using DataHandlers;
    using Interfaces;
    using Service;
    using UnityEngine;

    public abstract class DataPersistenceBase<T>: MonoBehaviour where T : class, new()
    {
        [Header("File Storage Config")] 
        [SerializeField] protected string fileName;
        [SerializeField] protected bool useEncryption;
        
        protected IDataHandler dataHandler;
        protected DataManager<T> dataManager;
        protected List<IDataPersistence<T>> persistenceObjects;
        protected PersistenceService<T> persistenceService;

        public abstract void SaveData();
        public abstract void LoadData();
    }
}