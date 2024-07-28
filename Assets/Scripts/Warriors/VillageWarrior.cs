using System;
using Unity.VisualScripting;
using UnityEngine;

// using DefaultNamespace;

namespace Warriors
{
    public class VillageWarrior : Warrior
    {
        private Warrior _enemyToEngage;
        protected bool fittedEnemyPosition = false;


        public new void FixedUpdate()
        {
            base.FixedUpdate();
            if (!isDead) HandleEnemyInteraction();
        }


        public override void ExitBattle()
        {
            base.ExitBattle();
            fittedEnemyPosition = false;
            inCombat = false;
            _enemyToEngage = null;
            curDirection = baseDirection;
        }

        public override void ResetWarrior()
        {
            base.ResetWarrior();
            _enemyToEngage = null;
        }

        private void HandleEnemyInteraction()
        {
            if (_enemyToEngage == null)
            {
                DetectEnemy();
            }

            else if (_enemyToEngage != null && !fittedEnemyPosition)
            {
                EngageEnemy();
            }
            else if (_enemyToEngage != null && fittedEnemyPosition && !inCombat)
            {
                if (Vector3.Distance(this.transform.position, _enemyToEngage.transform.position) < 1.3f)
                {
                    EnterBattle(_enemyToEngage);
                    _enemyToEngage.EnterBattle(this);
                    inCombat = true;
                }
            }
        }

        private void EngageEnemy()
        {
            float xOffset = 0.2f;
            if (_enemyToEngage.isDead || !_enemyToEngage.gameObject.activeSelf ||
                _enemyToEngage.transform.position.x + xOffset < transform.position.x)
            {
                _enemyToEngage = null;
                curDirection = baseDirection;
                return;
            }

            float yOffset = 0.07f;
            if (_enemyToEngage.transform.position.y > yOffset + transform.position.y)
            {
                curDirection = Vector2.up;
            }
            else if (_enemyToEngage.transform.position.y + yOffset < transform.position.y)
            {
                curDirection = Vector2.down;
            }
            else
            {
                fittedEnemyPosition = true;
                curDirection = baseDirection;
            }
        }

        private void DetectEnemy()
        {
            Collider2D[] overlappingObjects = Physics2D.OverlapCircleAll(transform.position, 3f);
            foreach (Collider2D col in overlappingObjects)
            {
                {
                    // print(gameObject.tag);
                    // print(col.gameObject.tag);
                    Warrior potentialEnemy = col.gameObject.GetComponent<Warrior>();

                    if (ValidWarrior(potentialEnemy))
                    {
                        _enemyToEngage = potentialEnemy;
                        return;
                    }
                }
            }
        }

        private bool ValidWarrior(Warrior warrior)
        {
            return warrior && !warrior.isDead &&
                   !warrior.gameObject.CompareTag(gameObject.tag) &&
                   transform.position.x < warrior.transform.position.x;
        }
    }
}