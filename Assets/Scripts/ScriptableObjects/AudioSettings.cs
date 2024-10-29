namespace RehvidGames.ScriptableObjects
{
    using Audio;
    using UnityEngine;

    [CreateAssetMenu(fileName = "AudioSettings", menuName = "Audio/Settings")]
    public class AudioSettings : ScriptableObject
    {
        public AudioCollection[] AudioCollections;
    }
}