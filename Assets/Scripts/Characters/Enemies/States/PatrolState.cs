namespace RehvidGames.Characters.Enemies.States
{
    using RehvidGames.AI.States.Interfaces;
    using Enemies;
    using Base;
    using Enums;
    using Factories;

    public class PatrolState : BaseState
    {
        public PatrolState(EnemyController enemyController) : base(enemyController) { }

        public override IState GetNextState()
        {
            return ShouldTransitionToChaseState() ? EnemyStateFactory.GetState(EnemyStateType.Chase, controller) : this;
        }

        public override void Enter()
        {
            base.Enter();

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