namespace RehvidGames.DataPersistence.Service
{
    using System.Collections.Generic;
    using DataHandlers;
    using Interfaces;
    using Managers;

    public abstract class PersistenceService<T> where T : class, new()
    {
        protected readonly IDataHandler dataHandler;
        protected readonly DataManager<T> dataManager;

        protected PersistenceService(IDataHandler dataHandler, DataManager<T> dataManager)
        {
            this.dataHandler = dataHandler;
            this.dataManager = dataManager;
        }
        
        public abstract void Load(List<IDataPersistence<T>> persistenceObjects);
        public abstract void Save(List<IDataPersistence<T>> persistenceObjects);

        protected void SaveToPersistenceObjects(List<IDataPersistence<T>> persistenceObjects)
        {
            if (!dataManager.HasData()) return; 
            
            foreach (var persistenceObject in persistenceObjects)
            {
                persistenceObject.SaveData(dataManager.Data);
            }
        }

        protected void LoadFromPersistenceObjects(List<IDataPersistence<T>> persistenceObjects)
        {
            foreach (var persistenceObject in persistenceObjects)
            {
                persistenceObject.LoadData(dataManager.Data);
            }
        }
    }
}