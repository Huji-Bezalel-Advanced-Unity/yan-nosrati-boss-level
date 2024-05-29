using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonWarrior : Warrior
{
    void Awake()
    {
        health = 100;
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
    
    public void AttackAnimationTriggered()
    {
        print(gameObject.tag);
        DamageEnemy(inCombatWith,damage);
    }
}
