namespace RehvidGames.Characters.Enemies
{
    using UnityEngine;

    public class EnemySightDetecting : MonoBehaviour
    {
        public Vector3 LastKnownLocationPlayer { get; private set; }
        
        public bool IsPlayerDetected { get; set; }
        
        public void OnPlayerSightDetected() => IsPlayerDetected = true;
        
        public void OnPlayerSightLost(Vector3 lastKnownLocationPlayer)
        {
            IsPlayerDetected = false;
            LastKnownLocationPlayer = lastKnownLocationPlayer;
        }
        
        public void SetDefaultLastKnownLocationPlayer() => LastKnownLocationPlayer = Vector3.zero;
        
        public bool HasLastKnownLocationPlayer() => LastKnownLocationPlayer != Vector3.zero;
    }
}