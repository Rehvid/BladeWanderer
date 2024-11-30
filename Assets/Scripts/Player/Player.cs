namespace RehvidGames.Player
{
    using Animator;
    using Character;
    using UnityEngine;
    using Enums;
    using Interfaces;
    using Managers;
    using ScriptableObjects;
    using Serializable;
    using UnityEngine.SceneManagement;
    using Weapons;

    public class Player: BaseCharacter, IDataPersistence
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

        public void LoadData(GameData data)
        {
            transform.position = data.PlayerGameData.Position;
            _attributes.AddSouls(data.PlayerGameData.CollectedSouls);
        }

        public void SaveData(GameData data)
        {
            data.PlayerGameData.Position = transform.position;
            data.PlayerGameData.CollectedSouls = _attributes.CurrentSouls;
            data.CurrentSceneName = SceneManager.GetActiveScene().name;
        }
    }
}