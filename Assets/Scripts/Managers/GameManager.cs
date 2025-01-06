namespace RehvidGames.Managers
{
    using Audio;
    using Characters.Player;
    using DG.Tweening;
    using UI.Menu;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Utilities;

    public class GameManager : BaseSingletonMonoBehaviour<GameManager>
    {
        [SerializeField] private GameObject _deathScreen;
        
        public PlayerController Player { get; private set; }

        public bool IsPaused { get; private set; }
        
        public bool IsPlayerDead { get; private set; }

        private void Start()
        {
            FindPlayer();
        }

        protected override void ResetState()
        {
            FindPlayer();
        }

        private void FindPlayer()
        {
            if (SceneManager.GetActiveScene().buildIndex == 0) return;
            
            GameObject.FindGameObjectWithTag("Player").TryGetComponent(out PlayerController playerController);
            Player = playerController;
        }

        public void OnDeath(Component sender, object value = null)
        {
           IsPlayerDead = true;
           Instantiate(_deathScreen);
        }
        
        public void LoadMainMenu()
        {
            SetTimeScale(1);
            ClearGameState();
            SceneManager.LoadScene(MainMenu.SceneName); 
        }

        private void ClearGameState()
        {
            StopAllSounds();
            DOTween.KillAll();
            IsPaused = false;
        }
     
        public void PauseGame()
        {
            if (IsPaused) return;
            IsPaused = true;
            
            StopAllSounds(); 
            SetTimeScale(0);
            DOTween.PauseAll(); 
        }

        public void ResumeGame()
        {
            if (!IsPaused) return;
            IsPaused = false;
            
            SetTimeScale(1); 
            DOTween.PlayAll();
        }
        
        private void SetTimeScale(int scale) => Time.timeScale = scale; 
        
        private void StopAllSounds() => AudioManager.Instance.StopAllSounds();
    }
}