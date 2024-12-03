namespace RehvidGames.DataPersistence.Managers
{
    using System.Collections.Generic;
    using Interfaces;

    public class DataPersistenceRegistryManager
    {
        private static DataPersistenceRegistryManager _instance;
        public static DataPersistenceRegistryManager Instance => _instance ??= new DataPersistenceRegistryManager();
        
        private List<IDataPersistence> _registeredObjects = new();
        
        private DataPersistenceRegistryManager() {}
        
        public void Register(IDataPersistence dataPersistenceObject)
        {
            if (!_registeredObjects.Contains(dataPersistenceObject))
            {
                _registeredObjects.Add(dataPersistenceObject);
            }
        }

        public void Unregister(IDataPersistence dataPersistenceObject)
        {
            if (_registeredObjects.Contains(dataPersistenceObject))
            {
                _registeredObjects.Remove(dataPersistenceObject);
            }
        }

        public List<IDataPersistence> GetAllRegisteredObjects()
        {
            return _registeredObjects;
        }
    }
}