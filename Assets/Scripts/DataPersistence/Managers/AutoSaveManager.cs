namespace RehvidGames.DataPersistence.Managers
{
    using System.Collections;
    using Data;
    using Data.State;
    using Helpers;
    using Interfaces;
    using RehvidGames.Managers;
    using Service;
    using UnityEngine;

    public class AutoSaveManager: MonoBehaviour
    {
        private bool _isAutoSaveEnabled = true;
        
        [Header("Auto Saving Configuration")]  
        [SerializeField] private float _autoSaveTimeSeconds = 360f;
        
        private PersistenceService<GameState> _gameStatePersistenceService;
        private Coroutine _autoSaveCoroutine;

        public void SetGameStatePersistenceService(PersistenceService<GameState> service)
        {
            _gameStatePersistenceService = service;
        }

        public void HandleAutoSaveCoroutine()
        {
            if (_autoSaveCoroutine != null)
            {
                StopCoroutine(_autoSaveCoroutine);
            }
            _autoSaveCoroutine = StartCoroutine(AutoSave());
        }

        public void StopAutoSaveCoroutine()
        {
            _isAutoSaveEnabled = false;
            if (_autoSaveCoroutine == null) return;
            
            StopCoroutine(_autoSaveCoroutine);
            _autoSaveCoroutine = null;
        }

        private IEnumerator AutoSave()
        {
            while (_isAutoSaveEnabled)
            {
                yield return new WaitForSeconds(_autoSaveTimeSeconds);
                _gameStatePersistenceService.Save(RegistryManager<IDataPersistence<GameState>>.Instance.RegisteredObjects);
                Debug.Log("Auto saving...");
            }
        }
    }
}