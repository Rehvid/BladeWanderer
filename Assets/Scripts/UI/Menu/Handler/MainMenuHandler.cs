namespace RehvidGames.UI.Menu.Handler
{
    using Managers;
    using UnityEngine;

    public class MainMenuHandler: MonoBehaviour
    {
        public void OnMainMenuClicked()
        {
            GameManager.Instance.LoadMainMenu();
        }
    }
}