namespace RehvidGames.Data.Serializable
{
    using RehvidGames.Data.Events.Events;
    using UnityEngine;
    using UnityEngine.Events;

    [System.Serializable]
    public class CustomGameEvent : UnityEvent<Component, object> {}
    
    public class GameEventListener: MonoBehaviour
    {
        public GameEvent GameEvent;
        public CustomGameEvent Response;
        
        private void OnEnable()
        {
            GameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            GameEvent.UnregisterListener(this);
        }

        public void OnEventRaised(Component sender, object value = null)
        {
            Response?.Invoke(sender, value);
        }
    }
}