namespace RehvidGames.Characters.Player
{
    using Animator;
    using Audio;
    using Audio.Data;
    using Base;
    using Enums;
    using Items.Weapons.Base;
    using Managers;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class PlayerWeaponHandler: BaseCharacterWeaponHandler
    {
        [Header("Weapon mounts")]
        [SerializeField] private Transform _rightHandMount;
        [SerializeField] private Transform _leftLegMount;
        
        // [Header("Weapon")]
        // [SerializeField] private BaseWeapon _weapon;
        
        [Header("Audio")]
        [SerializeField] private AudioData _weaponDrawAudioData;
        [SerializeField] private AudioData _weaponHideAudioData;
        
        public bool IsDrawnWeapon { get; private set; }
        // public BaseWeapon CurrentWeapon => _weapon;
        
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