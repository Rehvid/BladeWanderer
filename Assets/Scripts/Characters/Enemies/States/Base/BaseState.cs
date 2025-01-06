namespace RehvidGames.Characters.Enemies.States.Base
{
    using Enemies;
    using Enums;
    using Factories;
    using Interfaces;
    using UnityEngine;

    public abstract class BaseState: IState
    {
        protected EnemyController controller;
        
        public abstract void Execute();
        
        public abstract IState GetNextState();
        
        public virtual void Enter(){}
        
        public virtual void Exit(){}
        
        protected BaseState(EnemyController enemyController)
        {
            controller = enemyController;
        }
        
        protected bool ShouldTransitionToChaseState() => controller.Sight.IsPlayerInSight;
        
        
        protected IState GetStateBasedOnLastKnownPlayerLocation()
        {
            return EnemyStateFactory.GetState(
                HasLastKnownLocationPlayer() ? EnemyStateType.Search : EnemyStateType.Patrol,
                controller
            );
        }

        protected bool HasLastKnownLocationPlayer() => controller.Sight.PlayerLastKnownLocation != Vector3.zero;

        protected void LookAtPlayer() => controller.Enemy.transform.LookAt(controller.Player.transform);  
    }
}