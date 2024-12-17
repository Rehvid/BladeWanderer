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
        [SerializeField] private WeaponManager _weaponManager;
        
        public PlayerActionManager ActionManager => _actionManager;
        
        public PlayerAttributes Attributes => _attributes;
        
        public StaminaCosts StaminaCosts => _staminaCosts;

        private bool _isSpawned;
        
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
        
        public void LoadData(GameState data) //TODO: Refactor later
        {
            PlayerProfile playerProfile = data.PlayerProfile;
            PlayerAttribute stamina = playerProfile.Stamina;
            PlayerAttribute health = playerProfile.Health;
            
            transform.position = playerProfile.Position;
            
            _attributes.AddSouls(data.PlayerProfile.CollectedSouls);
            _attributes.Stamina.MaxValue = stamina.MaxValue;
            _attributes.Stamina.CurrentValue = stamina.CurrentValue;
            
            healthAttribute.MaxValue = health.MaxValue;
            healthAttribute.CurrentValue = health.CurrentValue;

            WeaponState weaponState = data.PlayerProfile.WeaponState;
            var socket = weaponState.IsCurrenltyEquipped ? _primaryWeaponSocket : _storageWeaponSocket;
            var weapon = _weaponManager.InstantiateWeapon("SwordOneHanded", socket);
            if (weapon)
            {
                Weapon = weapon;
                weapon.SetCurrentlyEquipped(weaponState.IsCurrenltyEquipped);
                var baseWeapons = FindObjectsByType<BaseWeapon>(FindObjectsSortMode.None); //OnScene
                if (baseWeapons.Length <= 0) return;
                foreach (var baseWeapon in baseWeapons)
                {
                    if (baseWeapon.Name == weaponState.Id && baseWeapon != weapon)
                    {
                        Destroy(baseWeapon.gameObject); 
                    }
                }
            }

            if (!_isSpawned)
            {
                 EnemyManager.Instance.Instantiate(EnemyType.Minotaur);
                 _isSpawned = true;
            }
        }

        public void SaveData(GameState data)
        {
            PlayerProfile playerProfile = data.PlayerProfile;
            
            playerProfile.Position = transform.position;
            playerProfile.CollectedSouls = _attributes.CurrentSouls;
            playerProfile.Stamina = new PlayerAttribute(_attributes.Stamina.MaxValue, _attributes.Stamina.CurrentValue);
            playerProfile.Health = new PlayerAttribute(healthAttribute.MaxValue, healthAttribute.CurrentValue);
            if (Weapon)
            { 
                playerProfile.WeaponState = new WeaponState(Weapon.Name, Weapon.IsCurrentlyEquipped);
            }
        }
    }
}