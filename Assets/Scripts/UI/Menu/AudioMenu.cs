namespace RehvidGames.UI.Menu
{
    using UnityEngine;
    using UnityEngine.Audio;

    public class AudioMenu : MonoBehaviour
    {
        [SerializeField] private AudioMixer _audioMixer;

        public void OnMasterVolume(float value)
        {
            _audioMixer.SetFloat("MasterVolume", value);
        }

        public void OnMusicVolume(float value)
        {
            _audioMixer.SetFloat("Music", value);
        }

        public void OnSFXVolume(float value)
        {
            _audioMixer.SetFloat("SFX", value);
        }
    }
}