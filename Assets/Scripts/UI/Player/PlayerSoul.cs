namespace RehvidGames.UI.Player
{
    using TMPro;
    using UnityEngine;

    public class PlayerSoul: MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        public void OnUpdateText(int souls)
        {
            _text.SetText(souls.ToString());
        }
    }
}