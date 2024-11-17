namespace RehvidGames.Collectable
{
    using UnityEngine;

    public abstract class Treasure: Collectable
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
        
        protected int GetRandomAmount() => Random.Range(minAmount, maxAmount);
    }
}