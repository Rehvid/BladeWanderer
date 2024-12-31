namespace RehvidGames.Factories
{
    using System;
    using System.Collections.Generic;
    using AI.States;
    using AI.States.Interfaces;
    using Characters.Enemies;
    using Characters.Enemies.States;
    using Enums;
    using UnityEngine.SceneManagement;

    public static class EnemyStateFactory
    {
        private static Dictionary<EnemyStateType, IState> _states = new();
        
        static EnemyStateFactory()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _states.Clear();  
        }

        public static IState GetState(EnemyStateType stateType, EnemyController controller)
        {
            return !_states.TryGetValue(stateType, out var state) ? Create(stateType, controller) : state;
        }

        private static IState Create(EnemyStateType stateType, EnemyController controller)
        {
            return stateType switch
            {
                EnemyStateType.Patrol => _states[stateType] = new PatrolState(controller),
                EnemyStateType.Chase => _states[stateType] = new ChaseState(controller),
                EnemyStateType.Search => _states[stateType] = new SearchState(controller),
                EnemyStateType.Combat => _states[stateType] = new CombatState(controller),
                _ => throw new ArgumentOutOfRangeException(nameof(stateType), stateType, null)
            };
        }
    }
}