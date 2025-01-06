namespace RehvidGames.DataPersistence.Base
{
    using System.Collections.Generic;
    using Managers;
    using DataHandlers;
    using Service;
    using Interfaces;
    using UnityEngine;

    public abstract class BaseDataPersistence<T>: MonoBehaviour where T : class, new()
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