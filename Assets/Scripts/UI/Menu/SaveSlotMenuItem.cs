namespace RehvidGames.UI.Menu
{
    using System;
    using Serializable;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class SaveSlotMenuItem : MonoBehaviour
    {
        [Header("Profile")] 
        [SerializeField] private string _profileId;
        
        public string ProfileId => _profileId;
        public bool HasData { get; private set; }
        
        [Header("Content")]
        [SerializeField] private GameObject _noDataContent;
        [SerializeField] private GameObject _hasDataContent;
        [SerializeField] private TextMeshProUGUI _localizationText;
        [SerializeField] private TextMeshProUGUI _timestampText;

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
                _noDataContent.SetActive(true);
                _hasDataContent.SetActive(false);
                if (_clearDataButton != null)
                {
                      _clearDataButton.gameObject.SetActive(false); 
                }
                HasData = false;
            }
            else
            {
                _noDataContent.SetActive(false);
                _hasDataContent.SetActive(true);
                if (_clearDataButton != null)
                {
                    _clearDataButton.gameObject.SetActive(true);
                }
                HasData = true;
                _localizationText.text = data.CurrentSceneName;
                _timestampText.text = DateTime.FromBinary(data.LastUpdated).ToLocalTime().ToString("dd.MM.yyyy HH:mm:ss"); 
                
            }
        }

        public void SetInteractable(bool interactable)
        {
            _saveSlotButton.interactable = interactable;
            _clearDataButton.interactable = interactable;
        }
    }
}