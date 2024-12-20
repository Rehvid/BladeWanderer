﻿namespace RehvidGames.DataPersistence.Data.State
{
    using System;
    using UnityEngine;

    [Serializable]
    public class PlayerProfile
    {
        public Vector3 Position;
        public int CollectedSouls;
        public PlayerAttribute Stamina;
        public PlayerAttribute Health;
        public WeaponState WeaponState;
    }
}