namespace RehvidGames.Characters.Enemies.States
{
    using Enemies;
    using Base;
    using Enums;
    using Factories;
    using Interfaces;

    public class ChaseState : BaseState
    {
        public ChaseState(EnemyController enemyController) : base(enemyController) { }

        public override IState GetNextState()
        {
            if (controller.IsPlayerDead()) return EnemyStateFactory.GetState(EnemyStateType.Patrol, controller);
            
            if (ShouldTransitionToChaseState())
            {
                return controller.Combat.CanAttack() ? EnemyStateFactory.GetState(EnemyStateType.Combat, controller) : this; 
            }

            return GetStateBasedOnLastKnownPlayerLocation();
        }
        
        public override void Execute()
        {
            if (!CanContinueChase()) return;
            
            var movement = GetMovement();
            
            LookAtPlayer();
            movement.SetDestination(controller.Player.transform.position);
            movement.SetChaseSpeed();
        }

        public override void Exit()
        {
            base.Exit();
            
            GetMovement().SetDefaultSpeed();
        }

        private EnemyMovement GetMovement() => controller.Movement;
        
        private bool CanContinueChase() => ShouldTransitionToChaseState(); 
    }
}