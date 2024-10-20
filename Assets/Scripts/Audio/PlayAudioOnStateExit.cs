namespace RehvidGames.Audio
{
    using Enums;
    using UnityEngine;

    public class PlayAudioOnStateExit : StateMachineBehaviour
    {
        [SerializeField] private SoundType _soundType;
        [SerializeField] private AudioSource _audioSource;
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            AudioManager.PlayRandomAudioOneShot(_soundType, _audioSource);
        }
    }
}