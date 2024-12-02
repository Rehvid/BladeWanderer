namespace RehvidGames.DataPersistence.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using RehvidGames.Interfaces;
    using UnityEngine;

    public class PersistenceObjectFinder: MonoBehaviour
    {
        public static List<IDataPersistence> FindAll()
        {
            //TODO: Problem with inavtice objects (check it)
            return FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .OfType<IDataPersistence>()
                .ToList();
        }
    }
}