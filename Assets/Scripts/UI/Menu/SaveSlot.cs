namespace RehvidGames.UI.Menu
{
    using DataPersistence.Data;
    using TMPro;
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

        public void SetData(GameData data)
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

        private void ShowDataState(GameData data)
        {
            _noDataPanel.SetActive(false);
            _dataAvailablePanel.SetActive(true);
            SetActiveClearButton(true);
            IsDataAvailable = true;
            
            GameMetaData metaData = data.GameMetaData;
            CurrentSceneName = metaData.CurrentSceneName;
            _sceneNameText.text = metaData.CurrentSceneName;
            _lastUpdatedText.text = metaData.GetFormattedLastUpdated();
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