using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Utilities;
using Managers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Warriors;

public class CharacterLoader : MonoBehaviour
{
    private Player _player;
    private Boss _stoneBoss;
    [SerializeField] private MapManager map;
    [SerializeField] private Volume _volume;

    //HealthBars
    private GameObject playerHealthBarUI;
    private GameObject bossHealthBarUI;

    private void Start()
    {
        DontDestroyOnLoad(transform.root);
    }

    public void LoadStoneBoss()
    {
        var bossHealthBar = Resources.Load<GameObject>("HealthBarMain");
        bossHealthBarUI = Instantiate(bossHealthBar, new Vector3(8, 9.5f, 0), Quaternion.identity);
        var boss = Resources.Load<Boss>("HadesBoss");
        _stoneBoss = Instantiate(boss, new Vector2(Constants.StartingBossXPossition, 0), Quaternion.identity);
        _stoneBoss.Init(bossHealthBarUI.transform.GetChild(2));
    }

    public void LoadPlayer()
    {
        var playerHealthBar = Resources.Load<GameObject>("HealthBarMain");

        playerHealthBarUI = Instantiate(playerHealthBar, new Vector3(8, -9.5f, 0), Quaternion.identity);
        var originalPlayer = Resources.Load<Player>("Player");
        _player = Instantiate(originalPlayer, Constants.BowPosition, Quaternion.identity);
        _player.Init(playerHealthBarUI.transform.GetChild(2));
    }

    public void LoadStoneBattle()
    {
        SceneManager.sceneLoaded += LoadStoneBattleCharacters;
        SceneManager.LoadScene("Main");
    }

    private void LoadStoneBattleCharacters(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= LoadStoneBattleCharacters;
        LoadPlayer();
        LoadStoneBoss();
        LoadStoneMap();
        OnLoadComplete();
    }

    private void LoadStoneMap()
    {
        Volume volume = Instantiate(_volume, Vector3.zero, Quaternion.identity);
        map = Instantiate(map, Vector3.zero, Quaternion.identity);
        map.Init(volume);
    }

    public void LoadFireBattle()
    {
        //todo
    }

    private void OnLoadComplete()
    {
        Destroy(this.gameObject);
        GameManager.Instance.wonGame = false;
        CastManager.Instance.StartCD();
    }
    
    public void ExitGame()
    {
        Application.Quit();   
    }

}