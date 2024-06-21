 using System;
 using System.Collections;
 using System.Collections.Generic;
 using System.Net.Mime;
 using DefaultNamespace;
 using Managers;
 using Spells;
 using TMPro;
 using Unity.Mathematics;
 using UnityEngine;
 using UnityEngine.SceneManagement;
 using UnityEngine.UI;
 using Button = UnityEngine.UIElements.Button;
 using Random = UnityEngine.Random;

 public class GameLoader : MonoBehaviour
 {
     [SerializeField] private GameLoaderUI loaderUI;
     [SerializeField] private MapManager mapManager;

        

        private Player _player;
        private Boss _boss;
        private CastManagerPlayer _playerCastManager;
        private CastManagerBoss _bossCastManager;
        public GameObject canvas;

        // cast manager related
        private Image warriorSpellImage;
        private TextMeshProUGUI warriorSpellCD;
        private Image fairyDustSpellImage;
        private TextMeshProUGUI fairyDuskSpellCD;
        private Image divineArrowSpellImage;
        private TextMeshProUGUI divineArrowSpellCD;
        
        //HealthBars
        private GameObject playerHealthBarUI;
        private GameObject bossHealthBarUI;
        
        //spells
        [SerializeField] private SummonVillageWarriorSpell _summonVillageWarriorSpell;
        [SerializeField] private BasicArrowSpell _basicArrowSpell;
        [SerializeField] private FairyDustSpell _fairyDustSpell;
        [SerializeField] private DivineArrowSpell _divineArrowSpell;
        [SerializeField] private SummonSkeletonWarriorSpell _summonSkeletonWarriorSpell;
        [SerializeField] private SummonSkeletonWarriorSpell _summonBigSkeletonWarriorSpell;

        private GameObject _tutorialPanel;
        
        private void Start()
        {
            DontDestroyOnLoad(canvas);
            StartLoadingAsync();
        }

        private void StartLoadingAsync()
        {
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(loaderUI.transform.root.gameObject);
           
            loaderUI.Init(100);

            StartCoroutine(LoadCoreManager());
            StartCoroutine(LoadGameManager());
            StartCoroutine(LoadCastManagers());
            StartCoroutine(LoadInputManager());
            StartCoroutine(LoadTutorialManager());

        }

        private IEnumerator LoadGameManager()
        {
            yield return new GameManager();
        }

      
        private IEnumerator LoadTutorialManager()
        {
            yield return new TutorialManager(_tutorialPanel);
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
            List<Spell> playerSpellsList = new List<Spell>() { _summonVillageWarriorSpell,_fairyDustSpell, _divineArrowSpell, _basicArrowSpell};
            List<Spell> bossSpellsList = new List<Spell>() { _summonSkeletonWarriorSpell,_summonBigSkeletonWarriorSpell};
            Dictionary<Spell, ValueTuple<Image, TextMeshProUGUI>> UIElements = new Dictionary<Spell, ValueTuple<Image, TextMeshProUGUI>>()
            {
                {_summonVillageWarriorSpell, ( warriorSpellImage, warriorSpellCD )},
                {_fairyDustSpell, ( fairyDustSpellImage, fairyDuskSpellCD )},
                {_divineArrowSpell, (divineArrowSpellImage, divineArrowSpellCD)}
            };
            _playerCastManager = new CastManagerPlayer(playerSpellsList, UIElements);
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
            LoadMapManager();
            loaderUI.AddProgress(30);

            StartCoroutine(OnLoadComplete()); //dummy coroutine
        }

        private void LoadUI()
        {
            var fairy = Resources.Load<GameObject>("FairyDustUI");
            var warrior = Resources.Load<GameObject>("WarriorUI");
            var divine = Resources.Load<GameObject>("DivineArrowUI");
            var panel = Resources.Load<GameObject>("TutorialPanel");


            var instantiatedFairy = Instantiate(fairy, new Vector3(250, 25, 0), Quaternion.identity);
            var instantiatedWarrior = Instantiate(warrior, new Vector3(180, 25, 0), Quaternion.identity);
            var instantiatedDivne = Instantiate(divine, new Vector3(320, 25, 0), Quaternion.identity);
            _tutorialPanel = Instantiate(panel, new Vector3(0, 0, 0), Quaternion.identity);

            instantiatedFairy.transform.SetParent(canvas.transform, false);
            instantiatedWarrior.transform.SetParent(canvas.transform, false);
            instantiatedDivne.transform.SetParent(canvas.transform, false);
            _tutorialPanel.transform.SetParent(canvas.transform,false);

            
            fairyDustSpellImage = instantiatedFairy.transform.GetChild(0).GetComponent<Image>();
            fairyDuskSpellCD = instantiatedFairy.GetComponentInChildren<TextMeshProUGUI>();

            warriorSpellImage = instantiatedWarrior.transform.GetChild(0).GetComponent<Image>();
            warriorSpellCD = instantiatedWarrior.GetComponentInChildren<TextMeshProUGUI>();
            
            divineArrowSpellImage = instantiatedDivne.transform.GetChild(0).GetComponent<Image>();
            divineArrowSpellCD = instantiatedDivne.GetComponentInChildren<TextMeshProUGUI>();
        }

        private void LoadBoss()
        {
            var bossHealthBar = Resources.Load<GameObject>("HealthBarMain");
            bossHealthBarUI = Instantiate(bossHealthBar, new Vector3(8, 9.5f, 0), Quaternion.identity);
            var boss = Resources.Load<Boss>("HadesBoss");
             _boss = Instantiate(boss, new Vector2(Constants.StartingBossXPossition, 0), Quaternion.identity);
             _boss.Init(_bossCastManager, bossHealthBarUI.transform.GetChild(2));
           
        }

        private void LoadPlayer()
        {
            var playerHealthBar = Resources.Load<GameObject>("HealthBarMain");
          
            playerHealthBarUI = Instantiate(playerHealthBar, new Vector3(8, -9.5f, 0), Quaternion.identity);
            var originalPlayer = Resources.Load<Player>("Player");
            _player = Instantiate(originalPlayer, Constants.BowPosition, Quaternion.identity);
            _player.Init(_playerCastManager, playerHealthBarUI.transform.GetChild(2));

        }
        private void LoadMapManager()
        {
            MapManager _mapManager = Resources.Load<MapManager>("MapManager");
            mapManager = Instantiate(_mapManager, Vector3.zero, Quaternion.identity);
        }

        private IEnumerator OnLoadComplete()
        {
            int count = 0;
            while (count < 20)
            {
                loaderUI.AddProgress(1);
                count++;
                yield return new WaitForSeconds(0.1f);
            }

            Destroy(loaderUI.transform.root.gameObject);
            Destroy(this.gameObject);
        }
    }
