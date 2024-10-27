namespace RehvidGames.AI.States
{
    using Enums;
    using Factories;
    using Interfaces;
    
    public abstract class BaseState: IState
    {
        protected AIController controller;
        private bool _active;

        public abstract void Execute();
        
        public abstract IState GetNextState();
        
        public virtual void Enter() => EnableState();
        
        public virtual void Exit() => DisableState();
        
        public bool IsActive() => _active;
        
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
        
        private void EnableState() => _active = true;
        
        private void DisableState() => _active = false;
    }
}