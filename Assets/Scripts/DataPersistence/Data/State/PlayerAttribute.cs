namespace RehvidGames.DataPersistence.Data.State
{
    using System;

    [Serializable]
    public class PlayerAttribute
    {
        public float MaxValue;
        public float CurrentValue;

        public PlayerAttribute (float maxValue, float currentValue)
        {
           MaxValue = maxValue;
           CurrentValue = currentValue;
        }
    }
}