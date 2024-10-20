namespace RehvidGames.AI
{
    using Player;
    using UnityEngine;

    public class AIDetecting : MonoBehaviour
    {
        [SerializeField] private Player _player;
        public Player Player => _player;
        public Vector3 LastKnownLocationPlayer { get; private set; }
        
        public bool IsPlayerDetected { get; private set; }

        public void OnPlayerSightDetected()
        {
            IsPlayerDetected = true;
        }

        public void OnPlayerSightLost(Vector3 lastKnownLocationPlayer)
        {
            IsPlayerDetected = false;
            LastKnownLocationPlayer = lastKnownLocationPlayer;
        }
        
        public void SetDefaultLastKnownLocationPlayer() => LastKnownLocationPlayer = Vector3.zero;
        
        public bool HasLastKnownLocationPlayer() => LastKnownLocationPlayer != Vector3.zero;
    }
}