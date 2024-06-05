using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonWarrior : Warrior
{
    void Awake()
    {
        health = 30;
        maxHealth = 100;
        attackTime = 1f;
        baseDirection = Vector2.left;
        curDirection = Vector2.left;
        inCombatWith = null;
        moveSpeed = 3f;
        damage = 20;
        stunned = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        base.FixedUpdate();

    }

  


    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        fittedEnemyPosition = true;
        curDirection = baseDirection;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Spell spell = other.gameObject.GetComponent<Spell>();
        if (!spell) return;

        foreach (Debuff d in spell.GetSpellsDebuffs())
        {
            if (d is DamageDebuff) d.Apply(this);
        }
        
    }
    
    public void AttackAnimationTriggered()
    {
        if (inCombatWith==null || inCombatWith.isDead) return;
        DamageEnemy(inCombatWith,damage);
    }
 
}
