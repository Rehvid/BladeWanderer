namespace RehvidGames.Audio.StateMachineBehavior
{
    using Enums;
    using UnityEngine;

    public class PlayAudioOnStateExit : StateMachineBehaviour
    {
        [SerializeField] private SoundType _soundType;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private string _clipName;
        
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var instance = AudioManager.Instance;
            if (string.IsNullOrEmpty(_clipName))
            {
                instance.PlayRandomClip(_soundType, _audioSource);    
                return;
            }
            
            instance.PlayClip(_soundType, _clipName, _audioSource);
        }
    }
}