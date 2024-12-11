namespace RehvidGames.Managers
{
    using System.Collections.Generic;
    using UnityEngine;

    public class RegistryManager <T> where T : class
    {
        public List<T> RegisteredObjects { get; } = new();
        public static RegistryManager<T> Instance => _instance ??= new RegistryManager<T>();
        
        private static RegistryManager<T> _instance;

        private RegistryManager()
        {
            RegisterMonoBehaviours();
        }
        
        public void RegisterMonoBehaviours () =>
            RegisterObjects(Object.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None));
        
        private void Register(T obj)
        {
            if (!RegisteredObjects.Contains(obj))
            {
                RegisteredObjects.Add(obj);
            }
        }

        public void Unregister(T obj)
        {
            if (RegisteredObjects.Contains(obj))
            {
                RegisteredObjects.Remove(obj);
            }
        }
        
        private void RegisterObjects(MonoBehaviour[] objects)
        {
            foreach (var obj in objects)
            {
                if (obj is T registrableObject)
                { 
                    Register(registrableObject);
                }
            }
        }
    }
}