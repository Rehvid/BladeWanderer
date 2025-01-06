namespace RehvidGames.Characters.Player
{
    using Animator;
    using Base;
    using Data;
    using DataPersistence.Data.State;
    using DataPersistence.Interfaces;
    using Enums;
    using Managers;
    using UnityEngine;
    using UnityEngine.AI;
    using UnityEngine.SceneManagement;
    using VFX;

    public class PlayerController: BaseCharacter, IDataPersistence<GameState>
    {
        [Header("Stats")]
        [SerializeField] private PlayerAttributes _attributes; 
        [SerializeField] private StaminaCostsData staminaCostsData;
        [SerializeField] private PlayerWeaponHandler _playerWeaponHandler;
        
        [Header("Player character effects")]
        [SerializeField] private PlayerCharacterEffectsData characterEffectsData;
        
        public PlayerActionHandler ActionHandler { get; private set; }
        public PlayerWeaponHandler WeaponHandler => _playerWeaponHandler;
        public StaminaCostsData StaminaCostsData => staminaCostsData;
        public PlayerCharacterEffectsData CharacterEffectsData => characterEffectsData;
        
        public PlayerAttributes Attributes => _attributes;
        
        protected override void Awake()
        {
            base.Awake();
            if (TryGetComponent(out PlayerActionHandler actionHandler))
            {
                ActionHandler = actionHandler;
            }
            else
            {
                Debug.LogError($"No component of type {typeof(PlayerActionHandler)} found");
            }
        }

        private void OnDestroy()
        {
            RegistryManager<IDataPersistence<GameState>>.Instance.Unregister(this);
        }

        public override void ReceiveDamage(float damage, Vector3 hitPosition)
        {
            health.ReceiveDamage(damage, hitPosition);
            VFXManager.Instance.PlayParticleEffect(CharacterEffectsData.HitTakenBloodVfx, hitPosition);
            
            if (!health.IsDead())
            {
                HandleHitDirection(hitPosition);
            }
        }

        private void HandleHitDirection(Vector3 hitPosition)
        {
            HitDirectionType directionType = hitDirectionAnalyzer.GetDirectionType(hitPosition, transform); 
            animatorHandler.SetTrigger(AnimatorParameter.HitDirection);
            animatorHandler.SetTrigger(hitDirectionAnalyzer.GetAnimatorParameterTypeByHitDirectionType(directionType));
        }
        
        public override void OnDeath(Component sender, object value = null)
        {
            GameManager.Instance.GameOver();
            
            ActionHandler.ChangeCurrentAction(PlayerActionType.Death);
            animatorHandler.SetTrigger(AnimatorParameter.Death);
            if (TryGetComponent(out NavMeshObstacle navMeshObstacle))
            {
                navMeshObstacle.enabled = true;
            }
        }

        public void LoadData(GameState data)
        {
            AttributeState healthAttribute = data.PlayerAttributeState.Health;
            health.CurrentValue = healthAttribute.CurrentValue;
            health.MaxValue = healthAttribute.MaxValue;

            if (data.SessionState.IndexScene != SceneManager.GetActiveScene().buildIndex) return;
            
            transform.position = data.PlayerState.Position;
            transform.rotation = data.PlayerState.Rotation;
        }

        public void SaveData(GameState data)
        {
            var healthAttribute = new AttributeState
            {
                CurrentValue = health.CurrentValue,
                MaxValue = health.MaxValue
            };

            PlayerState state = data.PlayerState;
            state.Position = transform.position;
            state.Rotation = transform.rotation;
            
            data.PlayerAttributeState.Health = healthAttribute;
        }
        
        #region proxy functions
        public bool HasEnoughStamina(float staminaCost) => _attributes.Stamina.HasEnoughStamina(staminaCost);
        
        public void UseStamina(float staminaCost) => _attributes.Stamina.UseStamina(staminaCost);

        public void RegenerationStamina() => _attributes.Stamina.Regeneration();

        public bool IsRegenerationStarted() => _attributes.Stamina.isRegenerationStarted();
        
        public void StopRegenerationStamina() => _attributes.Stamina.StopRegeneration();

        public bool IsWeaponEquipped() => _playerWeaponHandler.CurrentWeapon != null;

        public bool IsWeaponDrawn() => _playerWeaponHandler.IsDrawnWeapon;
        #endregion
    }
}