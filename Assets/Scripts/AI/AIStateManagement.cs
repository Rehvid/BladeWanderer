namespace RehvidGames.AI
{
    using Enums;
    using Factories;
    using Interfaces;
    using UnityEngine;

    public class AIStateManagement : MonoBehaviour
    {
        private IState _currentState;

        public void InitState(AIController aiController)
        {
            _currentState = AIStateFactory.GetState(AIStateType.Patrol, aiController);
            _currentState.Enter();
        }

        public void ExecuteCurrentState() => _currentState.Execute();
        
        public void TransitionToNextState()
        {
            Debug.Log(_currentState);
            var nextState = _currentState.GetNextState();
            if (nextState != _currentState)
            {
                TransitionToState(nextState);
            }
        }
        
        private void TransitionToState(IState state)
        {
            _currentState?.Exit();
            _currentState = state;
            _currentState.Enter();
        }
    }
}