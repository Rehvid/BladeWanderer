namespace RehvidGames.AI.States
{
    using Animator;
    using Data.Serializable;
    using Factories;
    using Enums;
    using Interfaces;
    
    public class FightState: BaseState
    {
        public FightState(AIController aiController) : base(aiController) { }
        
        public override IAIState GetNextState()
        {
            if (controller.IsPlayerDead())
            {
                return AIStateFactory.GetState(AIStateType.Patrol, controller);
            }
            
            if (!ShouldTransitionToChaseState())
            {
                return GetStateBasedOnLastKnownPlayerLocation();
            }

            return controller.CanAttack() ? this : AIStateFactory.GetState(AIStateType.Chase, controller);
        }

        public override void Execute()
        {
            controller.StopMovement();
            PlayAttackAnimation(true);
            controller.StartAttack();
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayAttackAnimation(false);
            controller.ResumeMovement();
            controller.StopAttack();
        }
        
        private void PlayAttackAnimation(bool value)
        {
            controller.PlayAnimation(
                AnimatorParameter.GetParameterName(AnimatorParameter.Attack),
                AnimatorParameterType.Bool, 
                value
            );
        }
    }
}