namespace RehvidGames.Characters.Enemies
{
    using States.Interfaces;
    using UnityEngine;

    public class EnemyStateHandler : MonoBehaviour
    {
        private IState _currentState;

        public void InitState(IState state)
        {
            _currentState = state;
            _currentState.Enter();
        }

        public void ExecuteCurrentState() => _currentState.Execute();
        
        public void TransitionToNextState()
        {
            var nextState = _currentState.GetNextState();
            if (nextState != _currentState)
            {
                TransitionToState(nextState);
            }
        }
        
        public void ExitCurrentState() => _currentState?.Exit();
        
        private void TransitionToState(IState state)
        {
            _currentState?.Exit();
            _currentState = state;
            _currentState.Enter();
        }
    }
}