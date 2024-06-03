 using System;
 using System.Collections;
 using System.Collections.Generic;
 using Managers;
 using Spells;
 using UnityEngine;
 using UnityEngine.SceneManagement;
 using Random = UnityEngine.Random;

 public class GameLoader : MonoBehaviour
    {
        [SerializeField]
        private GameLoaderUI loaderUI;

        private Player _player;
        private Boss _boss;
        private CastManagerPlayer _playerCastManager;
        private CastManagerBoss _bossCastManager;
        [SerializeField] private SummonVillageWarriorSpell _summonVillageWarriorSpell;
        [SerializeField] private SummonSkeletonWarriorSpell _summonSkeletonWarriorSpell;
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
            StartCoroutine(LoadCastManagers());
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

        private IEnumerator LoadCastManagers()
        {
            List<Spell> playerSpellsList = new List<Spell>() { _summonVillageWarriorSpell, _basicArrowSpell };
            List<Spell> bossSpellsList = new List<Spell>() { _summonSkeletonWarriorSpell};
            _playerCastManager = new CastManagerPlayer(playerSpellsList);
            _bossCastManager = new CastManagerBoss(bossSpellsList);
            yield return null;
        }

      

        private void OnCoreManagersLoaded(bool isSuccess)
        {
            if (isSuccess)
            {
                loaderUI.AddProgress(20);
                LoadMainScene();
            }
            else
            {
                Debug.LogException(new Exception("CoreManager failed to load"));
            }
        }
        

        private void LoadMainScene()
        {
            loaderUI.AddProgress(30);
            SceneManager.sceneLoaded += OnMainSceneLoaded;
            SceneManager.LoadScene("Main");

        }

        private void OnMainSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= OnMainSceneLoaded;
            
            LoadPlayer();
            LoadBoss();
            loaderUI.AddProgress(30);

            OnLoadComplete();
        }

        private void LoadBoss()
        {
            var boss = Resources.Load<Boss>("HadesBoss");
             _boss = Instantiate(boss, new Vector2(Constants.BossXPossition, 0), Quaternion.identity);
             _boss.Init(_bossCastManager);
        }

        private void LoadPlayer()
        {
            var originalPlayer = Resources.Load<Player>("Player");
            _player = Instantiate(originalPlayer, Constants.BowPosition, Quaternion.identity);
            _player.Init(_playerCastManager);

        }

        private void OnLoadComplete()
        {
            loaderUI.AddProgress(20);
            Destroy(loaderUI.transform.root.gameObject);
            Destroy(this.gameObject);
        }
    }
