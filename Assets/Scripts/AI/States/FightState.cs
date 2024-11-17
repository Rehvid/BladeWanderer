namespace RehvidGames.AI.States
{
    using Factories;
    using Enums;
    using Interfaces;

    public class FightState: BaseState
    {
        public FightState(AIController aiController) : base(aiController) { }
        
        public override IState GetNextState()
        {
            if (controller.IsAttacking()) return this;
            
            if (controller.IsPlayerDead()) return AIStateFactory.GetState(AIStateType.Patrol, controller);
            
            if (!ShouldTransitionToChaseState())
            {
                return GetStateBasedOnLastKnownPlayerLocation();
            }

            return controller.CanAttack() ? this : AIStateFactory.GetState(AIStateType.Chase, controller);
        }
        
        public override void Execute()
        {
            controller.StopMovement();
            controller.Attack();
        }

        public override void Exit()
        {
            controller.ResumeMovement();
        }
        
    }
}