namespace RehvidGames.Characters.Enemies.States
{
    using Enemies;
    using Base;
    using Enums;
    using Factories;
    using Interfaces;

    public class PatrolState : BaseState
    {
        public PatrolState(EnemyController enemyController) : base(enemyController) { }

        public override IState GetNextState()
        {
            return ShouldTransitionToChaseState() ? EnemyStateFactory.GetState(EnemyStateType.Chase, controller) : this;
        }

        public override void Enter()
        {
            var movementPatrol = GetMovementPatrol();
            
            if (!movementPatrol.IsImmediatePatrol)
            {
                movementPatrol.IsImmediatePatrol = true;
            }
        }

        public override void Execute()
        {
            controller.Movement.Patrol();
            var movementPatrol = GetMovementPatrol();
            if (movementPatrol.IsImmediatePatrol)
            {
                movementPatrol.IsImmediatePatrol = false;
            }
        }
        
        private EnemyMovementPatrol GetMovementPatrol() => controller.Movement.MovementPatrol;
    }
}