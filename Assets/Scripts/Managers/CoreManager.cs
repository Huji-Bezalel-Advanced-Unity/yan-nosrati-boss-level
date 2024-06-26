using System;

namespace Managers
{
    public class CoreManager
    {
        private Action<bool> _onComplete;
        public static CoreManager Instance;
        
        public CoreManager(Action<bool> onComplete)
        {
            if (Instance != null)
            {
                return;
            }
            
            Instance = this;
            _onComplete = onComplete;
            OnLoadSuccess();
        }

        private void OnLevelsManagerLoaded(bool obj)
        {
            OnLoadSuccess();
        }

        public void OnLoadSuccess()
        {
            _onComplete?.Invoke(true);
        }
        
        public void OnLoadFail()
        {
            _onComplete?.Invoke(false);
        }
    }
}