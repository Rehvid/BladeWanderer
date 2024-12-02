namespace RehvidGames.UI.Menu
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class ConfirmationPopupMenu: MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private TextMeshProUGUI _displayText;
        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _cancelButton;
        
        
        public void ActivateMenu(string displayText, UnityAction confirmAction, UnityAction cancelAction)
        {
            gameObject.SetActive(true);
            _displayText.text = displayText;
            
            _confirmButton.onClick.RemoveAllListeners();
            _cancelButton.onClick.RemoveAllListeners();
            
            _confirmButton.onClick.AddListener(() =>
            {
                DeactivateMenu();
                confirmAction();
            });
            _cancelButton.onClick.AddListener(() =>
            {
                DeactivateMenu();
                cancelAction();
            });
        }

        private void DeactivateMenu()
        {
            gameObject.SetActive(false);
        }
        
    }
}