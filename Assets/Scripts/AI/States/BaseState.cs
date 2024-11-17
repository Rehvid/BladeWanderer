namespace RehvidGames.AI.States
{
    using Enums;
    using Factories;
    using Interfaces;
    
    public abstract class BaseState: IState
    {
        protected AIController controller;
        
        public abstract void Execute();
        
        public abstract IState GetNextState();
        
        public virtual void Enter(){}
        
        public virtual void Exit(){}
        
        protected BaseState(AIController aiController)
        {
            controller = aiController;
        }
        
        protected bool ShouldTransitionToChaseState() => controller.IsPlayerDetected();
        
        protected IState GetStateBasedOnLastKnownPlayerLocation()
        {
            return AIStateFactory.GetState(
                controller.HasLastKnownLocationPlayer() ? AIStateType.Search : AIStateType.Patrol,
                controller
            );
        }
    }
}