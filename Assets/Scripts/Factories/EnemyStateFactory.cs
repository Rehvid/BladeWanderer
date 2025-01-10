namespace RehvidGames.Factories
{
    using System;
    using Characters.Enemies;
    using Characters.Enemies.States;
    using Characters.Enemies.States.Interfaces;
    using Enums;

    public static class EnemyStateFactory
    {
        public static IState Create(EnemyStateType stateType, EnemyController controller)
        {
            return stateType switch
            {
                EnemyStateType.Patrol => new PatrolState(controller),
                EnemyStateType.Chase => new ChaseState(controller),
                EnemyStateType.Search => new SearchState(controller),
                EnemyStateType.Combat => new CombatState(controller),
                _ => throw new ArgumentOutOfRangeException(nameof(stateType), stateType, null)
            };
        }
    }
}