namespace RehvidGames.AI.States
{
    using Factories;
    using Enums;
    using Interfaces;
    
    public class SearchState: BaseState
    { 
        public SearchState(AIController aiController) : base(aiController) { }
        
        public override IState GetNextState()
        {
            if (controller.IsPlayerDead()) return AIStateFactory.GetState(AIStateType.Patrol, controller);
            
           return ShouldSearchForPlayer() ? this : AIStateFactory.GetState(AIStateType.Patrol, controller);
        }

        public override void Execute()
        {
            if (!controller.HasLastKnownLocationPlayer()) return;
            controller.SetDestination(controller.LastKnownLocationPlayer);
        }

        public override void Exit()
        {
            base.Exit();
            controller.SetDefaultLastKnownLocationPlayer();
        }

        private bool ShouldSearchForPlayer()
        {
            return !controller.IsDestinationReach() && controller.HasLastKnownLocationPlayer();
        }
    }
}