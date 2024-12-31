namespace RehvidGames.Characters.Player
{
    using Enums;
    using UnityEngine;

    public class PlayerActionHandler: MonoBehaviour
    {
        private PlayerActionType _currentAction = PlayerActionType.Unoccupied;

        #region Events
        public void OnUnoccupiedAction() => _currentAction = PlayerActionType.Unoccupied;
        #endregion
        
        public void ChangeCurrentAction(PlayerActionType newAction) => _currentAction = newAction;
        
        public bool IsUnoccupied() => _currentAction == PlayerActionType.Unoccupied;
        
        public bool IsJumping() => _currentAction == PlayerActionType.Jumping;
    }
}