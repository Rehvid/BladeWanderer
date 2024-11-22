namespace RehvidGames.Serializable
{
    using System;
    using UnityEngine;

    [Serializable]
    public class AudioClipSettings
    {
        [Range(0f, 1f)] public float Volume = 1f;  
        [Range(-3f, 3f)] public float Pitch = 1f;    
        public bool Loop;  
        public string Name; 
        public AudioClip Clip;
    }
}