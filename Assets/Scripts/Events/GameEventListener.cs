namespace RehvidGames.Events
{
    using Data;
    using UnityEngine;
    using UnityEngine.Events;

    [System.Serializable]
    public class CustomGameEvent : UnityEvent<Component, object> {}
    
    public class GameEventListener: MonoBehaviour
    {
        public GameEventData gameEventData;
        public CustomGameEvent Response;
        
        private void OnEnable()
        {
            gameEventData.RegisterListener(this);
        }

        private void OnDisable()
        {
            gameEventData.UnregisterListener(this);
        }

        public void OnEventRaised(Component sender, object value = null)
        {
            Response?.Invoke(sender, value);
        }
    }
}