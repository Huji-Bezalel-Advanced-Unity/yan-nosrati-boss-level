 using System;
 using System.Collections;
 using System.Collections.Generic;
 using System.Net.Mime;
 using Managers;
 using Spells;
 using TMPro;
 using UnityEngine;
 using UnityEngine.SceneManagement;
 using UnityEngine.UI;
 using Button = UnityEngine.UIElements.Button;
 using Random = UnityEngine.Random;

 public class GameLoader : MonoBehaviour
    {
        [SerializeField]
        private GameLoaderUI loaderUI;

        private Player _player;
        private Boss _boss;
        private CastManagerPlayer _playerCastManager;
        private CastManagerBoss _bossCastManager;
        [SerializeField] private SummonSkeletonWarriorSpell _summonSkeletonWarriorSpell;
        [SerializeField] private SummonSkeletonWarriorSpell _summonBigSkeletonWarriorSpell;
        public GameObject canvas;

        // cast manager related
        private Image warriorSpellImage;
        private TextMeshProUGUI warriorSpellCD;
        private Image fairyDustSpellImage;
        private TextMeshProUGUI fairyDuskSpellCD;
        // [SerializeField] private Image DivineArrowSpellImage;
        // [SerializeField] private Text DivineArrowSpellCD;
        
        //HealthBars
        private GameObject playerHealthBarUI;
        private GameObject bossHealthBarUI;
        
        [SerializeField] private SummonVillageWarriorSpell _summonVillageWarriorSpell;
        [SerializeField] private BasicArrowSpell _basicArrowSpell;
        [SerializeField] private FairyDustSpell _fairyDustSpell;
        
        

        private void Start()
        {
            DontDestroyOnLoad(canvas);
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
            LoadUI();
            List<Spell> playerSpellsList = new List<Spell>() { _summonVillageWarriorSpell,_fairyDustSpell, _basicArrowSpell };
            List<Spell> bossSpellsList = new List<Spell>() { Instantiate(_summonSkeletonWarriorSpell), Instantiate(_summonBigSkeletonWarriorSpell)};
            Dictionary<Spell, ValueTuple<Image, TextMeshProUGUI>> UIElements = new Dictionary<Spell, ValueTuple<Image, TextMeshProUGUI>>()
            {
                {_summonVillageWarriorSpell, ( warriorSpellImage, warriorSpellCD )},
                {_fairyDustSpell, ( fairyDustSpellImage, fairyDuskSpellCD )}
            };
            foreach (var x in UIElements)
            {
                print(x.Key);
                print(x.Value);
                print("--");
            }

            _playerCastManager = new CastManagerPlayer(playerSpellsList, UIElements);
            _bossCastManager = new CastManagerBoss(bossSpellsList);
            print(playerHealthBarUI);
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

        private void LoadUI()
        {

            var fairy = Resources.Load<GameObject>("Button1");
            var warrior = Resources.Load<GameObject>("Button");

            var instantiatedFairy = Instantiate(fairy, new Vector3(250, 25, 0), Quaternion.identity);
            var instantiatedWarrior = Instantiate(warrior, new Vector3(180, 25, 0), Quaternion.identity);

            instantiatedFairy.transform.SetParent(canvas.transform, false);
            instantiatedWarrior.transform.SetParent(canvas.transform, false);
            
            fairyDustSpellImage = instantiatedFairy.transform.GetChild(0).GetComponent<Image>();
            fairyDuskSpellCD = instantiatedFairy.GetComponentInChildren<TextMeshProUGUI>();

            warriorSpellImage = instantiatedWarrior.transform.GetChild(0).GetComponent<Image>();
            warriorSpellCD = instantiatedWarrior.GetComponentInChildren<TextMeshProUGUI>();

        }

        private void LoadBoss()
        {
            var bossHealthBar = Resources.Load<GameObject>("HealthBarMain");
            bossHealthBarUI = Instantiate(bossHealthBar, new Vector3(8, 9.5f, 0), Quaternion.identity);
            var boss = Resources.Load<Boss>("HadesBoss");
             _boss = Instantiate(boss, new Vector2(Constants.BossXPossition, 0), Quaternion.identity);
             _boss.Init(_bossCastManager, bossHealthBarUI.transform.GetChild(2));
           
        }

        private void LoadPlayer()
        {
            var playerHealthBar = Resources.Load<GameObject>("HealthBarMain");
          
            playerHealthBarUI = Instantiate(playerHealthBar, new Vector3(8, -9.5f, 0), Quaternion.identity);
            print(playerHealthBarUI);
            var originalPlayer = Resources.Load<Player>("Player");
            _player = Instantiate(originalPlayer, Constants.BowPosition, Quaternion.identity);
            print(playerHealthBarUI);
            _player.Init(_playerCastManager, playerHealthBarUI.transform.GetChild(2));

        }

        private void OnLoadComplete()
        {
            loaderUI.AddProgress(20);
            Destroy(loaderUI.transform.root.gameObject);
            Destroy(this.gameObject);
        }
    }
