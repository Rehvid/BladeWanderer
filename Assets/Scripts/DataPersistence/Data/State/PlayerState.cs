namespace RehvidGames.DataPersistence.Data.State
{
    using System;
    using UnityEngine;

    [Serializable]
    public class PlayerState
    {
        public Vector3 Position;
        public Quaternion Rotation;
    }
}