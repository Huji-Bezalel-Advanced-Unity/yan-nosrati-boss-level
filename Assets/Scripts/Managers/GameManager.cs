
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager
    {
        public static GameManager Instance;

        public Action EndGame;

        public GameState gameState = GameState.Played;
        
        
        private bool _runTutorial = true;

        public GameManager()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            
        }
        
        public void SetRunTutorial()
        {
            _runTutorial = !_runTutorial;
        }

        public bool GetRunTutorial()
        {
            return _runTutorial;
        }

        public void WinGame()
        {
            gameState = GameState.Won;
            
            AudioManager.Instance.PlaySound(SoundName.WinSound);
            EventManager.Instance.InvokeEvent(EventNames.OnGameOver, EndGame);

        }

        public void LoseGame()
        {
            gameState = GameState.Lost; 
            
            AudioManager.Instance.PlaySound(SoundName.LoseSound);
            EventManager.Instance.InvokeEvent(EventNames.OnGameOver, EndGame);
        }

        public void StopTime()
        {
            Time.timeScale = 0;
        }
        
        public void ResumeTime()
        {
            Time.timeScale = 1;
        }

        public GameState GetGameState()
        {
            return gameState;
        }
        
        public void SetGameState(GameState newState)
        {
            gameState = newState;
        }

    }

    public enum GameState
    {
        Played = 1,
        Won = 2,
        Lost = 3,
    }
}