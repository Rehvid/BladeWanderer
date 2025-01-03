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
        [SerializeField] private PlayerController _player;
        [SerializeField] private GameObject _deathScreen;
        
        public PlayerController Player => _player;
        
        public bool IsPaused { get; private set; }
        
        public bool IsPlayerDead { get; private set; }

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
        }
     
        public void PauseGame()
        {
            if (IsPaused) return;
            IsPaused = true;
            
            StopAllSounds();
            SetTimeScale(0);
        }

        public void ResumeGame()
        {
            if (!IsPaused) return;
            IsPaused = false;
            
            SetTimeScale(1);
        }
        
        private void SetTimeScale(int scale) => Time.timeScale = scale; 
        
        private void StopAllSounds() => AudioManager.Instance.StopAllSounds();
    }
}