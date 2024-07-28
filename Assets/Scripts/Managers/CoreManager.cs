using System;
using System.Collections;
using UnityEngine;

namespace Managers
{
    public class CoreManager
    {
        public static CoreManager Instance;

        public MonoRunner MonoBehaviourRunner;
        private Action<bool> _onComplete;
        private int _loadCount = 0;
        private int _totalManagers = 5;

        public CoreManager(Action<bool> onComplete)
        {
            if (Instance != null)
            {
                return;
            }

            Instance = this;
            _onComplete = onComplete;
            MonoBehaviourRunner = new GameObject("CoreManagerRunner").AddComponent<MonoRunner>();
            MonoBehaviourRunner.StartCoroutine(LoadManagers());
        }

        private IEnumerator LoadManagers()
        {
            MonoBehaviourRunner.StartCoroutine(LoadEventSystem());
            MonoBehaviourRunner.StartCoroutine(LoadInputManager());
            MonoBehaviourRunner.StartCoroutine(LoadGameManager());
            MonoBehaviourRunner.StartCoroutine(LoadCastManager());
            MonoBehaviourRunner.StartCoroutine(LoadObjectPoolManager());

            yield return new WaitUntil(() => _loadCount >= _totalManagers);

            OnLoadSuccess();
        }

        private IEnumerator LoadEventSystem()
        {
            yield return null; // Simulate loading process
            new EventManager();
            _loadCount++;
        }

        private IEnumerator LoadObjectPoolManager()
        {
            yield return null; 
            new ObjectPoolManager();
            _loadCount++;
        }

        private IEnumerator LoadGameManager()
        {
            yield return null; 
            new GameManager();
            _loadCount++;
        }

        private IEnumerator LoadInputManager()
        {
            yield return null; 
            new InputManager();
            _loadCount++;
        }

        private IEnumerator LoadCastManager()
        {
            yield return null; 
            new CastManager();
            _loadCount++;
        }

        public void OnLoadSuccess()
        {
            _onComplete?.Invoke(true);
        }
    }
}
