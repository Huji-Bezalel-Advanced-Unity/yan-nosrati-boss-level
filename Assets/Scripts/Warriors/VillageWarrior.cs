using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
// using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;

public class VillageWarrior : Warrior
{

    void Awake()
    {
        health = 120;
        maxHealth = 120;
        attackTime = 2f;
        curDirection = Vector2.right;
        baseDirection = Vector2.right;
        inCombatWith= null;
        moveSpeed = 4f;
        damage = 20;
        stunned = false;
    }

    // Update is called once per frame
    public void FixedUpdate()
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

