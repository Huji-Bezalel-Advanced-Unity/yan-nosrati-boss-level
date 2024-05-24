 using System;
 using System.Collections;
 using System.Collections.Generic;
 using Managers;
 using UnityEngine;
 using UnityEngine.SceneManagement;
 using Random = UnityEngine.Random;

 public class GameLoader : MonoBehaviour
    {
        [SerializeField]
        private GameLoaderUI loaderUI;

        private Player _player;
        private CastManager _castManager;
        [SerializeField] private SummonVillageWarriorSpell _summonVillageWarriorSpell;
        [SerializeField] private BasicArrowSpell _basicArrowSpell;

        private void Start()
        {
            StartCoroutine(StartLoadingAsync());
        }

        private IEnumerator StartLoadingAsync()
        {
            yield return new WaitForSeconds(0.1f);
            
            
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(loaderUI.transform.root.gameObject);

            loaderUI.Init(100);

            StartCoroutine(LoadCoreManager());
            StartCoroutine(LoadCastManager());
            StartCoroutine(LoadInputManager());
        }

        private IEnumerator LoadInputManager()
        {
            yield return new InputManager();
        }

        private IEnumerator LoadCoreManager()
        {
            yield return new CoreManager(OnCoreManagersLoaded);
            
        }

        private IEnumerator LoadCastManager()
        {
            _castManager = new CastManager(_basicArrowSpell, _summonVillageWarriorSpell);
            yield return null;
        }

      

        private void OnCoreManagersLoaded(bool isSuccess)
        {
            if (isSuccess)
            {
                loaderUI.AddProgress(50);
                LoadMainScene();
            }
            else
            {
                Debug.LogException(new Exception("CoreManager failed to load"));
            }
        }
        

        private void LoadMainScene()
        {
            loaderUI.AddProgress(20);
            SceneManager.sceneLoaded += OnMainSceneLoaded;
            SceneManager.LoadScene("Main");

        }

        private void OnMainSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= OnMainSceneLoaded;
            
            LoadPlayer();
            LoadBoss();
            loaderUI.AddProgress(20);
            print($"4loader ui: {loaderUI}");


            OnLoadComplete();
        }

        private void LoadBoss()
        {
            var boss = Resources.Load<GameObject>("Boss");
            Instantiate(boss, new Vector2(Constants.BossXPossition, 0), Quaternion.identity);
        }

        private void LoadPlayer()
        {
            var originalPlayer = Resources.Load<Player>("Player");
            _player = Instantiate(originalPlayer, Constants.BowPosition, Quaternion.identity);
            _player.Init(_castManager);

        }

        private void OnLoadComplete()
        {
            loaderUI.AddProgress(10);
            Destroy(loaderUI.transform.root.gameObject);
            Destroy(this.gameObject);
        }
    }
