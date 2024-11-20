namespace RehvidGames.AI.States
{
    using Factories;
    using Enums;
    using Interfaces;
    public class ChaseState : BaseState
    {
        public ChaseState(AIController aiController) : base(aiController) { }
        
        public override IState GetNextState()
        {
            if (controller.IsPlayerDead()) return AIStateFactory.GetState(AIStateType.Patrol, controller);
            
            if (ShouldTransitionToChaseState())
            {
                return controller.CanAttack() ? AIStateFactory.GetState(AIStateType.Fight, controller) : this; 
            }

            return GetStateBasedOnLastKnownPlayerLocation();
        }
        
        public override void Execute()
        {
            if (!CanContinueChase()) return;
            controller.SetDestination(controller.DetectedPlayer.transform.position);
            controller.SetChaseSpeed();
        }

        public override void Exit()
        {
            base.Exit();
            controller.SetDefaultSpeed();
        }

        private bool CanContinueChase() => ShouldTransitionToChaseState(); 
    }
}