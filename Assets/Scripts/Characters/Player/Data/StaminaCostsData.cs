namespace RehvidGames.Characters.Player.Data
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewStaminaCosts", menuName = "Player/StaminaCosts")]
    public class StaminaCostsData : ScriptableObject
    {
        [Tooltip("Stamina cost of the running is multiplied by DeltaTime.")]
        [SerializeField] private float _baseRun = 5f;
        [SerializeField] private float _baseJump = 10f;
        [SerializeField] private float _baseDodge = 10f;

        public float BaseRun => _baseRun;

        public float BaseJump => _baseJump;

        public float BaseDodge => _baseDodge;
    }
}