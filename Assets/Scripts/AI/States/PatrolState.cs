namespace RehvidGames.AI.States
{
    using Factories;
    using Enums;
    using Interfaces;

    public class PatrolState : BaseState
    {
        public PatrolState(AIController aiController) : base(aiController) { }
        
        public override IState GetNextState()
        {
            
            
            return ShouldTransitionToChaseState() ? AIStateFactory.GetState(AIStateType.Chase, controller) : this;
        }

        public override void Enter()
        {
            base.Enter();
            if (!controller.GetImmediatePatrol())
            {
                 controller.SetImmediatePatrol(true);
            }
        }

        public override void Execute()
        {
            controller.StartPatrolling();
            if (controller.GetImmediatePatrol())
            {
                controller.SetImmediatePatrol(false);
            }
        }
        
    }
}