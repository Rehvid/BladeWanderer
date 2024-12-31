namespace RehvidGames.Characters.Enemies.States
{
    using RehvidGames.AI.States.Interfaces;
    using Enemies;
    using Base;
    using Enums;
    using Factories;

    public class CombatState: BaseState
    {
        public CombatState(EnemyController enemyController) : base(enemyController) { }

        public override IState GetNextState()
        {
            if (controller.Combat.CurrentAttackState != AttackStateType.Ready) return this; 
            
            if (controller.IsPlayerDead()) return EnemyStateFactory.GetState(EnemyStateType.Patrol, controller);
            
            if (!ShouldTransitionToChaseState())
            {
                return GetStateBasedOnLastKnownPlayerLocation();
            }

            return controller.Combat.CanAttack() ? this : EnemyStateFactory.GetState(EnemyStateType.Chase, controller);
        }
        
        public override void Execute()
        {
            controller.Movement.StopMovement();
            
            if (controller.Combat.CanAttack())
            {
                controller.Combat.Attack();
            } else if (controller.Combat.CanResetAttack())
            {
                controller.Combat.ResetAttack(); 
            }
        }
        
        

        public override void Exit()
        {
            controller.Movement.ResumeMovement();
            controller.Combat.SetReadyAttackState();
        }
    }
}