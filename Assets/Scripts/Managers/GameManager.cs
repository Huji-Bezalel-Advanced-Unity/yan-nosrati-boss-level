
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
        private bool runTutorial = true;
        
        public void setRunTutorial()
        {
            runTutorial = !runTutorial;
        }

        public bool GetRunTutorial()
        {
            return runTutorial;
        }
        
    }
}