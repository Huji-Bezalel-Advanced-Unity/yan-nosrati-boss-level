using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonWarrior : Warrior
{
    // Update is called once per frame
    void FixedUpdate()
    {
        base.FixedUpdate();

    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.GetComponent<Warrior>())
        {
            fittedEnemyPosition = true;
            curDirection = baseDirection;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Spell spell = other.gameObject.GetComponent<Spell>();
        if (!spell) return;

        foreach (Debuff d in spell.GetSpellsDebuffs())
        {
            if (d is not RevealDebuff) d.Apply(this);
        }
        
    }
    
    public void AttackAnimationTriggered()
    {
        
        if (inCombatWith==null || inCombatWith.isDead) return;
        DamageEnemy(inCombatWith,damage);
    }
 
}
