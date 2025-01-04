namespace RehvidGames.Characters.Player
{
    using Animator;
    using Audio;
    using Audio.Data;
    using Base;
    using DataPersistence.Data.State;
    using Enums;
    using Interfaces;
    using Items.Base;
    using Items.Weapons.Base;
    using Items.Weapons.Interfaces;
    using Managers;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class PlayerWeaponHandler: BaseCharacterWeaponHandler, IDataPersistence<GameState>
    {
        [Header("Weapon mounts")]
        [SerializeField] private Transform _rightHandMount;
        [SerializeField] private Transform _leftLegMount;
        
        [Header("Audio")]
        [SerializeField] private AudioData _weaponDrawAudioData;
        [SerializeField] private AudioData _weaponHideAudioData;
        
        public bool IsDrawnWeapon { get; private set; }
        
        private PlayerController _player;
        private AudioManager _audioManager;
        
        private void Start()
        {
            _player = GameManager.Instance.Player;
            
            if (_player == null)
            {
                Debug.LogWarning("Player not found!");
            }
            
            _audioManager = AudioManager.Instance;
            
            if (_audioManager == null)
            {
                Debug.LogWarning("No audio manager found!");
            }
        }

        public void LoadData(GameState data)
        {
            if (string.IsNullOrEmpty(data.InventoryState.weaponState.Id)) return;
            
            BaseWeapon baseWeapon = GetWeaponFromGameState(data.InventoryState.weaponState);
            if (baseWeapon)
            {
              HandleInstantiateWeapon(data.InventoryState.weaponState, baseWeapon);  
            }
        }

        private BaseWeapon GetWeaponFromGameState(WeaponState weaponState)
        {
            ItemManager itemManager = ItemManager.Instance;
            BaseItem itemScene = itemManager.FindItemInScene(weaponState.Id);
            
            if (itemScene != null)
            {
                Destroy(itemScene.gameObject);
            }
            
            return itemManager.InstantiateItem(weaponState.Id) as BaseWeapon;
        }

        private void HandleInstantiateWeapon(WeaponState weaponState, BaseWeapon baseWeapon)
        {
            if (baseWeapon is IPickUpWeapon)
            {
                var pickUp = baseWeapon as IPickUpWeapon;
                Destroy(pickUp.GetPickableWeaponCollider());
            }
            
            if (weaponState.IsDrawnWeapon)
            {
                AttachWeaponToRightHand(baseWeapon);
                return;
            }
            
            AttachWeaponToLeftLeg(baseWeapon);
        }
        
        public void SaveData(GameState data)
        {
            WeaponState weaponState = data.InventoryState.weaponState;
            if (!_weapon) return;
            
            weaponState.IsDrawnWeapon = IsDrawnWeapon;
            weaponState.Id = _weapon.Id;
        }
        

        #region Events
        public void OnWeaponTogglePerformed(InputAction.CallbackContext context)
        {
            if (context.performed && _weapon)
            {
                ToggleWeapon();
            }
        }
        
        private void OnPlayerWeaponDrawToRightHand()
        {
            AttachWeaponToRightHand(_weapon);
            _audioManager.PlayClip(_weaponDrawAudioData.SoundType, _weaponDrawAudioData.SoundName);
        }

        private void OnPlayerWeaponHideAudioPlay() => _audioManager.PlayClip(_weaponHideAudioData.SoundType, _weaponHideAudioData.SoundName); 
        
        private void OnPlayerWeaponAttachToLeftLeg() => AttachWeaponToLeftLeg(_weapon);
        #endregion
        
        public void AttachWeaponToRightHand(BaseWeapon weapon)
        {
            _weapon = weapon;
            weapon.Equip(_rightHandMount);
            IsDrawnWeapon = true;
        }

        public void AttachWeaponToLeftLeg(BaseWeapon weapon)
        {
            _weapon = weapon;
            _weapon.UnEquip(_leftLegMount);
            IsDrawnWeapon = false;
        }
        
        private void ToggleWeapon()
        {
            _player.ActionHandler.ChangeCurrentAction(PlayerActionType.Interacting);
            ToggleWeaponAnimation();
        }
        
        private void ToggleWeaponAnimation()
        {
            _player.AnimatorHandler.SetTrigger(AnimatorParameter.Interaction);
            _player.AnimatorHandler.SetTrigger(!IsDrawnWeapon ? AnimatorParameter.DrawWeapon : AnimatorParameter.HideWeapon);
            _player.AnimatorHandler.SetBool(AnimatorParameter.HasEquippedWeapon, !IsDrawnWeapon); 
        }
    }
}