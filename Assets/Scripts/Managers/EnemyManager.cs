namespace RehvidGames.Managers
{
    using System.Collections.Generic;
    using Characters.Base;
    using Characters.Enemies;
    using Characters.Enemies.Base;
    using Enums;
    using UnityEngine;
    using Utilities;
    using Random = UnityEngine.Random;

    public class EnemyManager: BaseSingletonMonoBehaviour<EnemyManager>
    {
        [Header("Enemies")]
        [SerializeField] private List<Transform> _spawnPoints;
        [SerializeField] private float _spawnRadius = 10f;
        
        [Header("Debug")]
        [SerializeField] private bool _isDebugEnabled;
        
        private readonly Dictionary<EnemyType, GameObject> _enemyPrefabs = new (); 
        
        protected override void Awake()
        {
            base.Awake();
            
            FindEnemyPrefabs();
        }
        
        private void FindEnemyPrefabs()
        {
            var enemies = Resources.LoadAll<BaseEnemy>("Enemies");
            foreach (var enemy in enemies)
            {
                if (!_enemyPrefabs.ContainsKey(enemy.GetEnemyType()))
                {
                    _enemyPrefabs.Add(enemy.GetEnemyType(), enemy.gameObject);
                }
            }
        }

        public void Instantiate(EnemyType type)
        {
            if (!_enemyPrefabs.TryGetValue(type, out GameObject prefab)) return;
            Transform spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Count)];
            Vector3 spawnPosition = GetRandomSpawnPosition(spawnPoint);
            
            if (_isDebugEnabled)
            {
                DrawDebugLines(spawnPoint, spawnPosition);
            }
            
            Instantiate(prefab, spawnPosition, Quaternion.identity);
        }

        private Vector3 GetRandomSpawnPosition(Transform spawnPoint)
        {
            var randomAngle = Random.Range(0f, Mathf.PI * 2);
            var randomDistance = Random.Range(0f, _spawnRadius);
            var xPosition = Mathf.Cos(randomAngle) * randomDistance;
            var zPosition = Mathf.Sin(randomAngle) * randomDistance;
            
            return new Vector3(xPosition, 0, zPosition) + spawnPoint.position; 
        }

        private void DrawDebugLines(Transform spawnPoint, Vector3 spawnPosition)
        {
            Debug.DrawLine(spawnPoint.position, spawnPosition, Color.blue, 15f);
        }
    }
}