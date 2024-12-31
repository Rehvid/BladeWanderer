namespace RehvidGames.Characters.Enemies
{
    using UnityEngine;
    using Utilities;

    public class EnemyTreasure : MonoBehaviour 
    {
        [SerializeField] private GameObject _treasure;
        
        public void CreateTreasureInstance(Transform treasureSpawnPoint)
        {
            if (_treasure == null) return; 
            
            var treasureInstance = Instantiate(
                _treasure, 
                treasureSpawnPoint.position,
                treasureSpawnPoint.rotation
            );

            if (treasureInstance.TryGetComponent(out WaveMotionController motionController))
            {
                motionController.SetStartPosition(treasureInstance.transform.position);
            }
        }
    }
}