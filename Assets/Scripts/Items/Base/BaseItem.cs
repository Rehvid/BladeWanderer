namespace RehvidGames.Items.Base
{
    using System;
    using Interfaces;
    using UnityEngine;

    public class BaseItem : MonoBehaviour, IItem
    {
        [Header("Item information")]
        [SerializeField] protected string id;

        public string Id => id;
        
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(id))
            {
                id = Guid.NewGuid().ToString();
            }
        }
    }
}