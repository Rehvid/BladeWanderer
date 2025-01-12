﻿namespace RehvidGames.Managers
{
    using System.Collections.Generic;
    using System.Linq;
    using Items.Base;
    using UnityEngine;
    using Utilities;
    
    public class ItemManager : BaseSingletonMonoBehaviour<ItemManager>
    {
        private readonly Dictionary<string, BaseItem> _itemsPrefabs = new();

        protected override void Awake()
        { 
            base.Awake();
            FindItemsInResources();
        }

        private void FindItemsInResources()
        {
            var items = Resources.LoadAll<BaseItem>("Items");
            if (items.Length <= 0) return;
            foreach (var item in items)
            {
                if (string.IsNullOrEmpty(item.Id)) continue;
                
                _itemsPrefabs.Add(item.Id, item);
            }
        }

        public BaseItem FindItemInScene(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var baseItems = FindObjectsByType<BaseItem>(FindObjectsSortMode.None);
            
            if (baseItems == null || baseItems.Length == 0) return null;
            
            return baseItems.FirstOrDefault(item => item.Id == id);
        }
        
        public BaseItem InstantiateItem(
            string id, 
            Vector3 position = default, 
            Quaternion rotation = default, 
            Transform parent = null)
        {
            if (!_itemsPrefabs.TryGetValue(id, out BaseItem item))
            {
                Debug.LogWarning($"Item with ID '{id}' not found!");
                return null;
            }
            
            if (position == default) position = Vector3.up;
            if (rotation == default) rotation = Quaternion.identity;
            
            BaseItem instantiated = Instantiate(item, position, rotation, parent);
            return instantiated;
        }
    }
}