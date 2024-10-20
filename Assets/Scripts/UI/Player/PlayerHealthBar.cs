namespace RehvidGames.UI.Player
{
    using UnityEngine;

    public class PlayerHealthBar : AttributeSlider
    {
        public void OnDeath(Component sender, object value = null)
        {
            Debug.Log("Handle Death - Player Bar");
        }
    }
}