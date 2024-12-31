namespace RehvidGames.Characters.Enemies.States
{
    using RehvidGames.AI.States.Interfaces;
    using Enemies;
    using Base;
    using Enums;
    using Factories;

    public class SearchState: BaseState
    {
        public SearchState(EnemyController enemyController) : base(enemyController) { }

        public override IState GetNextState()
        {
            if (controller.IsPlayerDead()) return EnemyStateFactory.GetState(EnemyStateType.Patrol, controller);
            
           return ShouldSearchForPlayer() ? this : EnemyStateFactory.GetState(EnemyStateType.Patrol, controller);
        }

        public override void Execute()
        {
            if (!HasLastKnownLocationPlayer()) return;
            
            controller.Movement.SetDestination(controller.Sight.PlayerLastKnownLocation);
        }

        public override void Exit()
        {
            base.Exit();
            
            controller.Sight.SetDefaultLastKnownLocationPlayer();
        }

        private bool ShouldSearchForPlayer()
        {
            return !controller.Movement.IsTargetDestinationReach() && HasLastKnownLocationPlayer();
        }
    }
}