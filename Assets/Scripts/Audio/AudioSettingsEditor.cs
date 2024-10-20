#if UNITY_EDITOR
    namespace RehvidGames.Audio
    {
        using System;
        using System.Collections.Generic;
        using Enums;
        using UnityEditor;
        using UnityEngine;

        [CustomEditor(typeof(AudioSettings))]
        public class AudioSettingsEditor : Editor
        {
            private bool _hasDifferentSize;
            
            private void OnEnable()
            {
                ref AudioCollection[] audioCollections = ref ((AudioSettings)target).AudioCollections;
                if (audioCollections == null) return;

                string[] audioTypeNames = Enum.GetNames(typeof(SoundType));
                
                _hasDifferentSize = audioTypeNames.Length != audioCollections.Length;
                
                Dictionary<string, AudioCollection> validAudioCollections = GetValidAudioCollections(audioCollections);
                
                Array.Resize(ref audioCollections, audioTypeNames.Length);
                
                UpdateAudioCollections(audioCollections, validAudioCollections, audioTypeNames);
            }

            private void UpdateAudioCollections(AudioCollection[] audioCollections, Dictionary<string, AudioCollection>validAudioCollections, string[] audioTypeNames)
            {
                for (int i = 0; i < audioCollections.Length; i++)
                {
                    string currentName = audioTypeNames[i];
                    audioCollections[i].Name = currentName;
                    
                    if (!_hasDifferentSize) continue;

                    if (validAudioCollections.TryGetValue(currentName, out var currentAudio))
                    {
                        UpdateElement(ref audioCollections[i], null, currentAudio.AudioSourceType);
                    }
                    else
                    {
                        UpdateElement(ref audioCollections[i], Array.Empty<AudioClipEntry>(), currentAudio.AudioSourceType);
                    }
                }
            }
            
            private void UpdateElement(ref AudioCollection element, AudioClipEntry[] clipEntry, AudioSourceType source)
            {
                element.Clips = clipEntry;
                element.AudioSourceType = source;
            }

            private Dictionary<string, AudioCollection> GetValidAudioCollections(AudioCollection[] audioCollections)
            {
                if (!_hasDifferentSize) return new Dictionary<string, AudioCollection>();
                Dictionary<string, AudioCollection> filteredAudioCollections = new();
                
                for (int i = 0; i < audioCollections.Length; ++i)
                {
                    var currentAudio = audioCollections[i];
                    if (string.IsNullOrEmpty(currentAudio.Name)) continue;
                    filteredAudioCollections.TryAdd(currentAudio.Name, currentAudio);
                }

                return filteredAudioCollections;
            }
        }
    }
#endif
