namespace RehvidGames.UI.Menu
{
    using DataPersistence.Data;
    using DataPersistence.Data.State;
    using TMPro;
    using Unity.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    public class SaveSlot : MonoBehaviour
    {
        [Header("Profile")] 
        [SerializeField] private string _profileId;
        
        public string ProfileId => _profileId;
        public string CurrentSceneName { get; private set; }
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
            _saveSlotButton = gameObject.GetComponent<Button>();
        }

        public void GenerateGuid()
        {
            _profileId = System.Guid.NewGuid().ToString();
        }

        public void SetData(GameState data)
        {
            if (data == null)
            {
                ShowNoDataState();
            }
            else
            {
                ShowDataState(data);
            }
        }

        private void ShowNoDataState()
        {
            _noDataPanel.SetActive(true);
            _dataAvailablePanel.SetActive(false);
            SetActiveClearButton(false);
            IsDataAvailable = false;
        }

        private void ShowDataState(GameState gameState)
        {
            _noDataPanel.SetActive(false);
            _dataAvailablePanel.SetActive(true);
            SetActiveClearButton(true);
            IsDataAvailable = true;
            
            GameSessionData sessionData = gameState.SessionData;
            
            CurrentSceneName = sessionData.CurrentSceneName;
            _sceneNameText.text = sessionData.CurrentSceneName;
            _lastUpdatedText.text = sessionData.GetFormattedLastUpdated();
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