using System.Threading.Tasks;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace DefaultNamespace
{
    public class WarriorBattle 
    {
        private Warrior _playerWarrior;
        private Warrior _enemyWarrior;
        public WarriorBattle(Warrior playerWarrior, Warrior enemyWarrior)
        {
            _playerWarrior = playerWarrior;
            _enemyWarrior = enemyWarrior;
        }

        public  async void RunBattle()
        { 
            _playerWarrior.EnterBattle();
            _enemyWarrior.EnterBattle();
            while (!_enemyWarrior.isDead && !_playerWarrior.isDead)
            {
                Debug.Log(_playerWarrior.isDead);
                DealDamage(_enemyWarrior, _playerWarrior.attackTime);
                await DealDamage(_playerWarrior, _enemyWarrior.attackTime);
            } 
            _playerWarrior.ExitBattle();
            _enemyWarrior.ExitBattle();
            
        }

        // DealDamage method needs to be async and awaitable
        private async Task DealDamage(Warrior enemyWarrior, float attackTime)
        {
            enemyWarrior.RemoveHealth(_playerWarrior.damage);
            await Task.Delay((int)(attackTime * 1000)); // Convert seconds to milliseconds
        }

        public async void Run1SidedBattle()
        {
            _playerWarrior.EnterBattle();
            while (!_enemyWarrior.isDead)
            {
                if (!_enemyWarrior.inCombat)
                {
                    RunBattle();
                    Debug.Log("run fight!");
                }
                 await DealDamage(_enemyWarrior, _playerWarrior.attackTime);
                 
            }
            _playerWarrior.ExitBattle();


        }
    }

}