namespace RehvidGames.UI.Menu
{
    using DataPersistence.Data;
    using DataPersistence.Data.State;
    using Managers;
    using TMPro;
    using Unity.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    public class SaveSlot : MonoBehaviour
    {
        [Header("Profile")] 
        [SerializeField] private string _profileId;
        
        public string ProfileId => _profileId;
        
        public int IndexScene { get; private set; }
        
        public bool IsDataAvailable { get; private set; }
        
        [Header("Content")]
        [SerializeField] private GameObject _noDataPanel;
        [SerializeField] private GameObject _dataAvailablePanel;
        [SerializeField] private TextMeshProUGUI _sceneNameText;
        [SerializeField] private TextMeshProUGUI _lastUpdatedText;

        [Header("Clear data button")]
        [SerializeField] private Button _clearDataButton;
        
        private Button _saveSlotButton;
        
        private void Awake()
        {
            FindSaveSlotButton();
        }

        public void GenerateGuid()
        {
            _profileId = System.Guid.NewGuid().ToString();
        }

        public void FindSaveSlotButton()
        {
            _saveSlotButton ??= gameObject.GetComponent<Button>();
        }
        
        public void SetData(GameState data)
        {
            var isDataAvailable = data != null;
            
            UpdateDataState(isDataAvailable);
            if (isDataAvailable)
            {
                UpdateSlotInformationFromSessionState(data);
            }

            if (GameManager.Instance.IsPaused)
            {
                SetActiveClearButton(false);
            }
        }

        private void UpdateDataState(bool isDataAvailable)
        {
            _noDataPanel.SetActive(!isDataAvailable);
            _dataAvailablePanel.SetActive(isDataAvailable);
            
            SetActiveClearButton(isDataAvailable);
            IsDataAvailable = isDataAvailable;
        }
        
        private void UpdateSlotInformationFromSessionState(GameState gameState)
        {
            GameSessionState sessionState = gameState.SessionState;
            
            IndexScene = sessionState.IndexScene;
            _sceneNameText.text = sessionState.CurrentSceneName;
            _lastUpdatedText.text = sessionState.GetFormattedLastUpdated();
        }

        public void SetInteractable(bool interactable)
        {
            _saveSlotButton.interactable = interactable;
            _clearDataButton.interactable = interactable; 
        }
        
        private void SetActiveClearButton(bool active)
        { 
            _clearDataButton.gameObject.SetActive(active);
        }
    }
}