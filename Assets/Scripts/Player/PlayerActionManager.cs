namespace RehvidGames.Player
{
    using Enums;
    using UnityEngine;
    
    public class PlayerActionManager: MonoBehaviour
    {
        private PlayerActionType _currentPlayerAction = PlayerActionType.Unoccupied;
        
        public void ChangeCurrentAction(PlayerActionType newAction)
        {
            _currentPlayerAction = newAction;
        }
        
        public bool IsUnoccupied()
        {
            return _currentPlayerAction == PlayerActionType.Unoccupied;
        }
        
        public bool IsJumping()
        {
            return _currentPlayerAction == PlayerActionType.Jumping;
        }
    }
}
