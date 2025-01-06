namespace RehvidGames.Events.Data
{
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewGameEvent", menuName = "Events/GameEvent<Component, object[]>")]
    public class GameEventData: ScriptableObject
    {
        public List<GameEventListener> Listeners = new();

        public void Raise(Component sender, object value = null)
        {
            for (int i = 0; i < Listeners.Count; i++)
            {
                Listeners[i].OnEventRaised(sender, value);
            }
        }

        public void RegisterListener(GameEventListener listener)
        {
            if (!Listeners.Contains(listener))
            {
                Listeners.Add(listener);
            }
        }

        public void UnregisterListener(GameEventListener listener)
        {
            if (Listeners.Contains(listener))
            {
                Listeners.Remove(listener);
            }
        }
    }
}