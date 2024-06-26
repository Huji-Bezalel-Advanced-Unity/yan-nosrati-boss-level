using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Warriors
{
    public class SkeletonWarrior : Warrior
    {
        // Update is called once per frame
        private new void FixedUpdate()
        {
            base.FixedUpdate();
        }

        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            if (isDead) return;
            CheckForSpellHit(collision2D.gameObject);
            if (inCombat || isDead) return;
            Warrior warrior = collision2D.gameObject.GetComponent<Warrior>();
            if (warrior && !inCombatWith.Contains(warrior) && !warrior.isDead)
            {
                EnterBattle(warrior);
                warrior.EnterBattle(this);
            }

        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            CheckForSpellHit(other.gameObject);
        }

        private void CheckForSpellHit(GameObject collision2D)
        {
            Spell spell = collision2D.gameObject.GetComponent<Spell>();
            if (spell) spell.ApllySpellDebuffs(this);
            
        }

        public void AttackAnimationTriggered()
        {
            if (inCombatWith.Count == 0 || inCombatWith.Peek().isDead) return;
            inCombatWith.Peek().damageFlash.CallFlasher();
            DamageEnemy(inCombatWith.Peek(), damage);

        }

        public override void ExitBattle()
        {
            base.ExitBattle();
            FightNext();
        }

        public void FightNext()
        {
            if (inCombatWith.Count > 0)
            {
                Fight();
            }
        }
    }
}