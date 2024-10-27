namespace RehvidGames.Factories
{
    using AI;
    using System;
    using System.Collections.Generic;
    using AI.States;
    using Interfaces;
    using Enums;

    public static class AIStateFactory
    {
        private static Dictionary<AIStateType, IState> _states = new();

        public static IState GetState(AIStateType stateType, AIController aiController)
        {
            return !_states.TryGetValue(stateType, out var state) ? Create(stateType, aiController) : state;
        }

        private static IState Create(AIStateType stateType, AIController aiController)
        {
            return stateType switch
            {
                AIStateType.Patrol => _states[stateType] = new PatrolState(aiController),
                AIStateType.Chase => _states[stateType] = new ChaseState(aiController),
                AIStateType.Search => _states[stateType] = new SearchState(aiController),
                AIStateType.Fight => _states[stateType] = new FightState(aiController),
                _ => throw new ArgumentOutOfRangeException(nameof(stateType), stateType, null)
            };
        }
    }
}