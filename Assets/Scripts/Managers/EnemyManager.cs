namespace RehvidGames.Managers
{
    using System.Collections.Generic;
    using Enemy;
    using Enums;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class EnemyManager: MonoBehaviour
    {
        public static EnemyManager Instance { get; private set; }
        
        [SerializeField] private List<Transform> _spawnPoints;
        [SerializeField] private float _spawnRadius = 10f;
        [SerializeField] private bool _isDebugEnabled = false;
        
        private readonly Dictionary<EnemyType, GameObject> _enemyPrefabs = new (); 
        
        private void Awake()
        {
            InitializeInstance();
            FindEnemyPrefabs();
        }

        private void InitializeInstance()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void FindEnemyPrefabs()
        {
            var enemies = Resources.LoadAll<BaseEnemy>("Enemies");
            foreach (var enemy in enemies)
            {
                if (!_enemyPrefabs.ContainsKey(enemy.GetType()))
                {
                    _enemyPrefabs.Add(enemy.GetType(), enemy.gameObject);
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