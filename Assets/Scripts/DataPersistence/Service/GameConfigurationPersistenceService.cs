namespace RehvidGames.DataPersistence.Service
{
    using System.Collections.Generic;
    using Data.Configuration;
    using DataHandlers;
    using Interfaces;
    using Managers;

    public class GameConfigurationPersistenceService: PersistenceService<GameConfiguration>
    {
        public GameConfigurationPersistenceService(IDataHandler dataHandler, DataManager<GameConfiguration> dataManager) 
            : base(dataHandler, dataManager) { }


        public override void Load(List<IDataPersistence<GameConfiguration>> persistenceObjects)
        {
            dataManager.Data = dataHandler.LoadGameConfiguration();
            if (!dataManager.HasData())
            {
                dataManager.ResetData();
            }
            LoadFromPersistenceObjects(persistenceObjects);
        }

        public override void Save(List<IDataPersistence<GameConfiguration>> persistenceObjects)
        {
            SaveToPersistenceObjects(persistenceObjects);
            dataHandler.SaveGameConfiguration(dataManager.Data);
        }
    }
}