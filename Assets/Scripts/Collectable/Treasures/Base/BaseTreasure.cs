namespace RehvidGames.Collectable.Treasures.Base
{
    using Collectable.Base;
    using UnityEngine;

    public abstract class BaseTreasure: BaseCollectable
    {
        [Header("Treasure properties")]
        [SerializeField] protected int minAmount = 1;
        [SerializeField] protected int maxAmount = 5;
        [SerializeField] protected bool useRandomAmount;
        [Tooltip("If 'useRandomAmount' is disabled, this value will define the amount of treasure.")]
        [SerializeField] protected int notRandomizingAmount = 3;
        
        [Header("Treasure audio")] 
        [SerializeField] protected AudioClip collectSound;
        
        protected int GetTreasureAmount() => useRandomAmount ? GetRandomAmount() : notRandomizingAmount;

        private int GetRandomAmount() => Random.Range(minAmount, maxAmount);
    }
}