using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageWarrior : Warrior
{
    private Warrior enemyToEngage;
    private bool fittedEnemyPosition;

    void Awake()
    {
        health = 80;
        maxHealth = 80;
        attackTime = 1.5f;
        direction = Vector2.right;
        inCombat = false;
        moveSpeed = 4f;
        damage = 15;
        stunned = false;
        enemyToEngage = null;
        fittedEnemyPosition = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!inCombat)  Move();

        if (enemyToEngage == null)
        {
            DetectEnemy();
        }

        if (enemyToEngage != null && !fittedEnemyPosition)
        {
            print("calling engage");
            EngageEnemy();
        }
        else if (enemyToEngage != null && fittedEnemyPosition)
        {
            if (Vector3.Distance(this.transform.position, enemyToEngage.transform.position) < 1.6f)
            {
                inCombat = true;
                animator.SetBool("Move", false);
                StartCoroutine(Battle(enemyToEngage));
            }
        }

        

    }

    private IEnumerator Battle(Warrior enemy)
    {
        if (enemy.isDead)
        {
            direction = Vector2.right;
            inCombat = false;
            animator.SetBool("Move", true);

        }
        animator.SetBool("InCombat",true);

        while (!this.isDead && !enemy.isDead)
        {
            yield return new WaitForSeconds(1);
        }
    }

    private void DetectEnemy()
    {
        Collider2D[] overlappingObjects =  Physics2D.OverlapCircleAll(transform.position, 7f);
        foreach (Collider2D col in overlappingObjects)
        {
            if (col.gameObject.CompareTag("Enemy"))
            {
                enemyToEngage = col.gameObject.GetComponent<Warrior>();
            }
        }


    }

    private void EngageEnemy()
    {
        if (enemyToEngage.transform.position.y > 0.1f + transform.position.y)
        {
            direction = Vector2.up;
        }
        else if (enemyToEngage.transform.position.y +0.1f < transform.position.y)
        {
            direction = Vector2.down;
        }
        else
        {
            fittedEnemyPosition = true;
            direction = Vector2.right;
        }
         
             
         


    }
}
//
// public class WarriorBattle
// {
//     public WarriorBattle(Warrior villageWarrior, Warrior enemyWarrior)
//     {
//     }
//
//     public IEnumerator run()
//     {
//         yield return null;
//         
//     }
// }
