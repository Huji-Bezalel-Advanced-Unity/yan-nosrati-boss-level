 using System;
 using System.Collections;
 using System.Collections.Generic;
 using Managers;
 using Spells;
 using TMPro;
 using UnityEngine;
 using UnityEngine.SceneManagement;
 using UnityEngine.UI;
 using Warriors;


 public class GameLoader : MonoBehaviour
 { 
    [SerializeField] private GameLoaderUI loaderUI;
    private Player _player;
    private Boss _boss;

    //HealthBars
    private GameObject playerHealthBarUI;
    private GameObject bossHealthBarUI;

    private void Start()
    {
    StartLoadingAsync();
    loaderUI.Init(100);

    }

    private void StartLoadingAsync()
    {
    DontDestroyOnLoad(gameObject);
    DontDestroyOnLoad(loaderUI.transform.root.gameObject);
    StartCoroutine(LoadCoreManager());
    }

    private IEnumerator LoadCoreManager()
    {
    yield return new CoreManager(OnCoreManagersLoaded);

    }

    private void OnCoreManagersLoaded(bool isSuccess)
    {
    if (isSuccess)
    {
        loaderUI.AddProgress(20);
        
        StartCoroutine(LoadMainScene());
    }
    else
    {
        Debug.LogException(new Exception("CoreManager failed to load"));
    }
    }


    private IEnumerator LoadMainScene()
    {
    int count = 0; 
    while (count < 30)
    {
        loaderUI.AddProgress(1);
        yield return new WaitForSeconds(0.1f);
        count++;
    }
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
    OnLoadComplete(); //dummy coroutine
    }

    private void LoadBoss()
    {
    var bossHealthBar = Resources.Load<GameObject>("HealthBarMain");
    bossHealthBarUI = Instantiate(bossHealthBar, new Vector3(8, 9.5f, 0), Quaternion.identity);
    var boss = Resources.Load<Boss>("HadesBoss");
     _boss = Instantiate(boss, new Vector2(Constants.StartingBossXPossition, 0), Quaternion.identity);
     _boss.Init(bossHealthBarUI.transform.GetChild(2));
    }

    private void LoadPlayer()
    {
    var playerHealthBar = Resources.Load<GameObject>("HealthBarMain");

    playerHealthBarUI = Instantiate(playerHealthBar, new Vector3(8, -9.5f, 0), Quaternion.identity);
    var originalPlayer = Resources.Load<Player>("Player");
    _player = Instantiate(originalPlayer, Constants.BowPosition, Quaternion.identity);
    _player.Init(playerHealthBarUI.transform.GetChild(2));

    }
    private void LoadMapManager()
    {
    MapManager _mapManager = Resources.Load<MapManager>("MapManager");
    Instantiate(_mapManager, Vector3.zero, Quaternion.identity);
    }

    private void OnLoadComplete()
    {
    Destroy(loaderUI.transform.root.gameObject);
    Destroy(this.gameObject);
    }
 }
