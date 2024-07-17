
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager
    {
        public static GameManager Instance;

        public Action EndGame;

        public bool wonGame = false;
        

        public GameManager()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            
        }
       
        private bool _runTutorial = true;
        
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
            AudioManager.Instance.PlaySound(SoundName.WinSound);
            wonGame = true;
            EventManager.Instance.InvokeEvent(EventNames.OnGameOver, EndGame);

        }

        public void LoseGame()
        {
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

    }
}