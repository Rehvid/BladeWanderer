﻿namespace RehvidGames.Audio.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using Serializable;
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewAudioCollection", menuName = "Sound/AudioCollection")]
    public class AudioCollectionData : ScriptableObject
    {
        public List<SoundCategory> SoundCategories;
        
        private void OnValidate()
        {
            if (SoundCategories == null) return;
            
            var duplicates = SoundCategories
                .GroupBy(category => category.SoundType)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key)
                .ToList();

            if (duplicates.Any())
            {
                Debug.LogError(
                    $"Duplicate SoundType entries found in {name}: {string.Join(", ", duplicates)}"
                );
            }
        }
    }
}