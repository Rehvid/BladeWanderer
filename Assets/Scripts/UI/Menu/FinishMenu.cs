namespace RehvidGames.UI.Menu
{
    using TMPro;
    using UnityEngine;

    public class FinishMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _finishScreen;
        [SerializeField] private TextMeshProUGUI _soulStatsText;

        public void SetSoulStatsText(int souls) => _soulStatsText.text = $"Dusze: {souls.ToString()}";
        
        public void InitializeFinishScreen() => Instantiate(_finishScreen);
    }
}