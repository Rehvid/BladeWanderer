namespace RehvidGames.Utilities
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// Base class for creating a singleton MonoBehaviour.
    /// Ensures only one instance exists. Use `_isPersistent` to keep the object between scenes.
    /// </summary>
    public class BaseSingletonMonoBehaviour<T>: MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] private bool _isPersistent;
        
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;
                
                _instance = FindAnyObjectByType<T>();
                
                if (_instance != null) return _instance;
                
                var singletonGameObject = new GameObject(typeof(T).Name + " Auto Generated");
                _instance = singletonGameObject.AddComponent<T>();

                return _instance;
            }
        }
        
        /// <summary>
        /// Call base.Awake() in override if you need awake.
        /// </summary>
        protected virtual void Awake()
        {
            InitializeSingleton();
        }
        
        private void OnEnable()
        {
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }

        private void OnDisable()
        {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        }

        private void OnActiveSceneChanged(Scene previousScene, Scene newScene)
        {
            ResetState();
        }

        protected virtual void ResetState() {}
        
        private void InitializeSingleton()
        {
            if (_instance == null)
            {
                _instance = this as T;
                if (_isPersistent)
                {
                   DontDestroyOnLoad(gameObject);  
                }
            } else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}