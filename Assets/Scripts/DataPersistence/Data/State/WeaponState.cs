namespace RehvidGames.DataPersistence.Data.State
{
    using System;
    
    [Serializable]
    public class WeaponState
    {
        public bool IsCurrenltyEquipped;
        public string Id;

        public WeaponState(string id, bool isCurrentlyEquipped)
        {
            Id = id;
            IsCurrenltyEquipped = isCurrentlyEquipped;
        }
    }
}