namespace RehvidGames.DataPersistence.Managers
{
    using System.Collections.Generic;
    using Interfaces;

    public class GameSettingsPersistenceRegistryManager
    {
        private static GameSettingsPersistenceRegistryManager _instance;
        public static GameSettingsPersistenceRegistryManager Instance 
            => _instance ??= new GameSettingsPersistenceRegistryManager();
        
        private List<IGameSettingsPersistence> _registeredObjects = new();
        
        private GameSettingsPersistenceRegistryManager() {}
        
        public void Register(IGameSettingsPersistence gameSettingsPersistenceObject)
        {
            if (!_registeredObjects.Contains(gameSettingsPersistenceObject))
            {
                _registeredObjects.Add(gameSettingsPersistenceObject);
            }
        }

        public void Unregister(IGameSettingsPersistence gameSettingsPersistenceObject)
        {
            if (_registeredObjects.Contains(gameSettingsPersistenceObject))
            {
                _registeredObjects.Remove(gameSettingsPersistenceObject);
            }
        }

        public List<IGameSettingsPersistence> GetAllRegisteredObjects()
        {
            return _registeredObjects;
        }
    }
}