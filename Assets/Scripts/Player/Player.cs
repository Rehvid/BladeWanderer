namespace RehvidGames.Player
{
    using Animator;
    using Character;
    using DataPersistence.Data;
    using DataPersistence.Data.State;
    using DataPersistence.Managers;
    using UnityEngine;
    using Enums;
    using Interfaces;
    using Managers;
    using ScriptableObjects;
    using Serializable;
    using UnityEngine.SceneManagement;
    using Weapons;

    public class Player: BaseCharacter, IDataPersistence<GameState>
    {
        [Header("Player configuration")]
        [SerializeField] private PlayerAttributes _attributes;
        [SerializeField] private Transform _primaryWeaponSocket;
        [SerializeField] private Transform _storageWeaponSocket;
        [SerializeField] private PlayerActionManager _actionManager;
        [SerializeField] private StaminaCosts _staminaCosts;
        
        public PlayerActionManager ActionManager => _actionManager;
        
        public PlayerAttributes Attributes => _attributes;
        
        public StaminaCosts StaminaCosts => _staminaCosts;
        
        private void OnDestroy()
        {
            RegistryManager<IDataPersistence<GameState>>.Instance?.Unregister(this);
        }

        public override void ReceiveDamage(float damage, Vector3 hitPosition)
        {
            healthAttribute.ReceiveDamage(damage, hitPosition);
            HitDirectionType directionType = hitDirectionAnalyzer.GetDirectionType(hitPosition, transform); 
            VFXManager.Instance.PlayParticleEffect(CharacterEffects.HitVfx, hitPosition);
            animatorHandler.SetTrigger(AnimatorParameter.HitDirection);
            animatorHandler.SetTrigger(hitDirectionAnalyzer.GetAnimatorParameterTypeByHitDirectionType(directionType));
        }
        
        public bool HasEquippedWeapon()
        {
            return Weapon != null && Weapon.IsCurrentlyEquipped;
        }
        
        public void AttachWeaponToPrimarySocket(BaseWeapon weapon)
        {
            Weapon = weapon;
            Weapon.Equip(_primaryWeaponSocket);
        }
        
        public void AttachWeaponToStorageSocket()
        {
            Weapon.UnEquip(_storageWeaponSocket);
        }
        
        public void SetAction(PlayerActionType actionType) => _actionManager.ChangeCurrentAction(actionType);

        #region StaminaAttribute shortcut functions
        public bool HasEnoughStamina(float staminaCost) => _attributes.Stamina.HasEnoughStamina(staminaCost);
        
        public void UseStamina(float staminaCost) => _attributes.Stamina.UseStamina(staminaCost);

        public void RegenerationStamina() => _attributes.Stamina.Regeneration();

        public bool IsRegenerationStarted() => _attributes.Stamina.isRegenerationStarted();
        
        public void StopRegenerationStamina() => _attributes.Stamina.StopRegeneration();
        #endregion
     
        
        public void OnDeath(Component sender, object value = null)
        {
            Debug.Log("OnDeath - Player"); 
        }
        
        public void LoadData(GameState data)
        {
            PlayerProfile playerProfile = data.PlayerProfile;
            
            transform.position = playerProfile.Position;
            _attributes.AddSouls(data.PlayerProfile.CollectedSouls);
        }

        public void SaveData(GameState data)
        {
            PlayerProfile playerProfile = data.PlayerProfile;
            
            playerProfile.Position = transform.position;
            playerProfile.CollectedSouls = _attributes.CurrentSouls;
        }
    }
}