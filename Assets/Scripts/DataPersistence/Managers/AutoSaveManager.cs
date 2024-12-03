namespace RehvidGames.DataPersistence.Managers
{
    using System.Collections;
    using Helpers;
    using UnityEngine;

    public class AutoSaveManager: MonoBehaviour
    {
        private bool _isAutoSaveEnabled = true;
        
        [Header("Auto Saving Configuration")]  
        [SerializeField] private float _autoSaveTimeSeconds = 360f;
        
        private SaveLoadManager _saveLoadManager;
        private Coroutine _autoSaveCoroutine;

        public void SetSaveLoadService(SaveLoadManager saveLoadManager)
        {
            _saveLoadManager = saveLoadManager;
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
                _saveLoadManager.SaveGame(DataPersistenceRegistryManager.Instance.GetAllRegisteredObjects());
                Debug.Log("Auto saving...");
            }
        }
    }
}