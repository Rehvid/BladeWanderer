namespace RehvidGames.ScriptableObjects
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "StaminaCosts", menuName = "Player/StaminaCosts")]
    public class StaminaCosts : ScriptableObject
    {
        [Tooltip("Stamina cost of the running is multiplied by DeltaTime.")]
        public float Run = 5f;
        public float Jump = 10f;
        public float Dodge = 10f;
    }
}