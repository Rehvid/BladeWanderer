namespace RehvidGames.DataPersistence.Data.State
{
    using System;

    [Serializable]
    public class PlayerAttributeState
    {
        public AttributeState Health;
        public AttributeState Stamina;
        public int CollectedSouls;
    }
}