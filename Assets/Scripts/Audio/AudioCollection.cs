namespace RehvidGames.Audio
{
    using System;
    using Enums;
    using UnityEngine;
    
    [Serializable]
    public struct AudioCollection
    {
        [HideInInspector] public string Name;
        public AudioSourceType AudioSourceType;
        public AudioClipEntry[] Clips;
    }
    
    [Serializable]
    public class AudioClipEntry
    {
        public string ClipName; 
        public AudioClip Clip;
    }
}