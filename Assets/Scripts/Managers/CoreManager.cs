using System;

namespace Managers
{
    public class CoreManager
    {
        private Action<bool> _onComplete;
        public static CoreManager instance;
        
        public CoreManager(Action<bool> onComplete)
        {
            if (instance != null)
            {
                return;
            }
            
            instance = this;
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