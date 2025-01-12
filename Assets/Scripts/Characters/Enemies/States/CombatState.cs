﻿namespace RehvidGames.Characters.Enemies.States
{
    using Enemies;
    using Base;
    using Enums;
    using Factories;
    using Interfaces;

    public class CombatState: BaseState
    {
        public CombatState(EnemyController enemyController) : base(enemyController) { }

        public override IState GetNextState()
        {
            if (controller.Combat.CurrentAttackState != AttackStateType.Ready) return this; 
            
            if (controller.IsPlayerDead()) return GetState(EnemyStateType.Patrol);
            
            if (!ShouldTransitionToChaseState())
            {
                return GetStateBasedOnLastKnownPlayerLocation();
            }

            return controller.Combat.CanAttack() ? this : GetState(EnemyStateType.Chase);
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
            LookAtPlayer();
        }
        
        public override void Exit()
        {
            controller.Movement.ResumeMovement();
            controller.Combat.SetReadyAttackState();
        }
    }
}