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
            List<Spell> bossSpellsList = new List<Spell>() { _summonSkeletonWarriorSpell};
            Dictionary<Spell, ValueTuple<Image, TextMeshProUGUI>> UIElements = new Dictionary<Spell, ValueTuple<Image, TextMeshProUGUI>>()
            {
                {_summonVillageWarriorSpell, ( warriorSpellImage, warriorSpellCD )},
                {_fairyDustSpell, ( fairyDustSpellImage, fairyDuskSpellCD )}
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
            loaderUI.AddProgress(30);

            OnLoadComplete();
        }

        private void LoadUI()
        {

            var fairy = Resources.Load<GameObject>("Button1");
            var warrior = Resources.Load<GameObject>("Button");
            var playerHealthBar = Resources.Load<GameObject>("HealthBar");
            var bossHealthBar = Resources.Load<GameObject>("HealthBar");
            bossHealthBarUI = Instantiate(bossHealthBar, new Vector3(171, 276, 0), Quaternion.identity);
            playerHealthBarUI = Instantiate(playerHealthBar, new Vector3(171, -130, 0), Quaternion.identity);
            fairyDustSpellImage = Instantiate(fairy.gameObject.GetComponentInChildren<Image>(), new Vector3(280,30,0), Quaternion.identity);
            fairyDuskSpellCD = Instantiate(fairy.gameObject.GetComponentInChildren<TextMeshProUGUI>(), new Vector3(280,30,0), Quaternion.identity);
            warriorSpellImage = Instantiate(warrior.gameObject.GetComponentInChildren<Image>(), new Vector3(230,30,0), Quaternion.identity);
            warriorSpellCD = Instantiate(warrior.gameObject.GetComponentInChildren<TextMeshProUGUI>(), new Vector3(230,30,0), Quaternion.identity);
            fairyDustSpellImage.transform.SetParent(canvas.transform, false);
            fairyDuskSpellCD.transform.SetParent(canvas.transform, false);
            warriorSpellImage.transform.SetParent(canvas.transform, false);
            warriorSpellCD.transform.SetParent(canvas.transform, false);
            playerHealthBarUI.transform.SetParent(canvas.transform, false);
            bossHealthBarUI.transform.SetParent(canvas.transform, false);
        }

        private void LoadBoss()
        {
            var boss = Resources.Load<Boss>("HadesBoss");
             _boss = Instantiate(boss, new Vector2(Constants.BossXPossition, 0), Quaternion.identity);
             _boss.Init(_bossCastManager, bossHealthBarUI.transform.Find("MissingHealth").GetComponent<Image>());
           
        }

        private void LoadPlayer()
        {
            var originalPlayer = Resources.Load<Player>("Player");
            _player = Instantiate(originalPlayer, Constants.BowPosition, Quaternion.identity);
            _player.Init(_playerCastManager, playerHealthBarUI.transform.Find("MissingHealth").GetComponent<Image>());

        }

        private void OnLoadComplete()
        {
            loaderUI.AddProgress(20);
            Destroy(loaderUI.transform.root.gameObject);
            Destroy(this.gameObject);
        }
    }
