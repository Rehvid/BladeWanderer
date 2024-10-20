namespace RehvidGames.Audio
{
    using Enums;
    using UnityEngine;

    public class PlayAudioOnStateEnter : StateMachineBehaviour
    {
        [SerializeField] private SoundType _soundType;
        [SerializeField] private AudioSource _audioSource;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            AudioManager.PlayRandomAudioOneShot(_soundType, _audioSource);
        }
    }
}
