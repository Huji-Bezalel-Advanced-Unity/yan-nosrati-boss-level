using Bosses;
using Managers;
using player;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace LoaderLogic
{
    public class CharacterLoader : MonoBehaviour
    {
        [SerializeField] private MapManager map; // this needs to be instantiated accordinly, not serilized.
        [SerializeField] private Volume _volume;

        private Player _player;
        private Boss _stoneBoss;
        private GameObject playerHealthBarUI;
        private GameObject bossHealthBarUI;


        private void Start()
        {
            DontDestroyOnLoad(transform.root);
        }


        public void LoadStoneBattle()
        {
            SceneManager.sceneLoaded += LoadStoneBattleCharacters;
            SceneManager.LoadScene("Main");
            
        }
        private void LoadStoneBattleCharacters(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= LoadStoneBattleCharacters;
            StartGame();
            LoadPlayer();
            LoadStoneBoss();
            LoadStoneMap();
            CastManager.Instance.StartCd();

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

        public void ExitGame()
        {
            Application.Quit();
        }
        

        private void StartGame()
        {
            Destroy(gameObject);
            GameManager.Instance.SetGameState(GameState.Played);
        }

        private void LoadStoneBoss()
        {
            Vector3 bossHealthBarPosition = new Vector3(8, 9.5f, 0);

            var bossHealthBar = Resources.Load<GameObject>("HealthBarMain");
            bossHealthBarUI = Instantiate(bossHealthBar, bossHealthBarPosition, Quaternion.identity);

            var boss = Resources.Load<Boss>("HadesBoss");
            _stoneBoss = Instantiate(boss, Constants.BossStartingPosition, Quaternion.identity);

            _stoneBoss.Init(bossHealthBarUI.transform.GetChild(2));
        }

        private void LoadPlayer()
        {
            Vector3 playerHealthBarPosition = new Vector3(8, -9.5f, 0);

            var playerHealthBar = Resources.Load<GameObject>("HealthBarMain");
            playerHealthBarUI = Instantiate(playerHealthBar, playerHealthBarPosition, Quaternion.identity);

            var originalPlayer = Resources.Load<Player>("Player");
            _player = Instantiate(originalPlayer, Constants.BowPosition, Quaternion.identity);

            _player.Init(playerHealthBarUI.transform.GetChild(2));
        }
    }
}