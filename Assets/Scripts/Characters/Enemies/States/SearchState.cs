namespace RehvidGames.Characters.Enemies.States
{
    using Enemies;
    using Base;
    using Enums;
    using Factories;
    using Interfaces;

    public class SearchState: BaseState
    {
        public SearchState(EnemyController enemyController) : base(enemyController) { }

        public override IState GetNextState()
        {
            if (controller.IsPlayerDead()) return GetState(EnemyStateType.Patrol);
            
           return ShouldSearchForPlayer() ? this : GetState(EnemyStateType.Patrol);
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