namespace RehvidGames.DataPersistence.Data.State
{
    using System;
  
    [Serializable]
    public class InventoryState
    {
        public WeaponState weaponState = new();
    }
}