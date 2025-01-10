namespace RehvidGames.Characters.Enemies
{
    using System;
    using System.Collections.Generic;
    using Enums;
    using Factories;
    using States.Interfaces;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class EnemyStateHandler : MonoBehaviour
    {
        private Dictionary<EnemyStateType, IState> _states = new();
        private IState _currentState;

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _states.Clear();  
        }

        public IState GetState(EnemyStateType stateType, EnemyController controller)
        {
            if (_states.TryGetValue(stateType, out var state)) return state;
            
            state = EnemyStateFactory.Create(stateType, controller);
            _states[stateType] = state;
            
            return state;
        }
        
        public void InitState(IState state)
        {
            _currentState = state;
            _currentState.Enter();
        }

        public void ExecuteCurrentState()
        {
            _currentState.Execute();
        } 
        
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