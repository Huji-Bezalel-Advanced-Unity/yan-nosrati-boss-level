
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager
    {
        public static GameManager Instance;

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
        
    }
}