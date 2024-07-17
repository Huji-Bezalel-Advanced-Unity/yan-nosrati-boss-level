﻿using System;
using System.Threading.Tasks;

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
            LoadManagers();
        }

        private async void LoadManagers()
        {
            var eventSystemManager = LoadEventSystem();
            var inputManagerTask = LoadInputManager();
            var gameManagerTask = LoadGameManager();
            var castManagerTask = LoadCastManager();
            var objectPoolManager = LoadObjectPoolManager();

            await Task.WhenAll(inputManagerTask, gameManagerTask, castManagerTask, objectPoolManager,
                eventSystemManager);

            OnLoadSuccess();
        }

        private async Task LoadEventSystem()
        {
            await Task.Delay(1000); // Simulate asynchronous loading
            new EventManager();
        }

        private async Task LoadObjectPoolManager()
        {
            await Task.Delay(1000); // Simulate asynchronous loading
            new ObjectPoolManager();
        }

        private async Task LoadGameManager()
        {
            await Task.Delay(1000); // Simulate asynchronous loading
            new GameManager();
        }

        private async Task LoadInputManager()
        {
            await Task.Delay(1000);
            new InputManager();
        }

        private async Task LoadCastManager()
        {
            await Task.Delay(1000);
            new CastManager();
        }


        public void OnLoadSuccess()
        {
            _onComplete?.Invoke(true);
        }
    }
}