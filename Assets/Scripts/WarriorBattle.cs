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
            bool first = true;
            _playerWarrior.EnterBattle();
            _enemyWarrior.EnterBattle();
            while (!_enemyWarrior.isDead && !_playerWarrior.isDead)
            {
                if (first)
                {
                    DealDamage(_enemyWarrior,_playerWarrior);
                    DealDamage(_playerWarrior,_enemyWarrior);
                    first = false;
                }
                await DealDamage(_enemyWarrior,_playerWarrior);
               await DealDamage(_playerWarrior,_enemyWarrior);
            } 
            _playerWarrior.ExitBattle();
            _enemyWarrior.ExitBattle();
            
        }

        // DealDamage method needs to be async and awaitable
        private async Task DealDamage(Warrior attackedWarrior, Warrior attackingWarrior)
        {
            attackedWarrior.RemoveHealth(attackedWarrior.damage);
            await Task.Delay((int)(attackingWarrior.attackTime * 1000)); // Convert seconds to milliseconds
        }

        public async void Run1SidedBattle()
        {
            _playerWarrior.EnterBattle();
            while (!_enemyWarrior.isDead)
            {
                if (!_enemyWarrior.inCombat)
                {
                    RunBattle();
                }
                 await DealDamage(_enemyWarrior, _playerWarrior);
                 
            }
            _playerWarrior.ExitBattle();


        }
    }

}