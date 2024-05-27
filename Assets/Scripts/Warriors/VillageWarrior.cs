using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;

public class VillageWarrior : Warrior
{
    private Warrior enemyToEngage;
    private bool fittedEnemyPosition;

    void Awake()
    {
        health = 120;
        maxHealth = 120;
        attackTime = 2f;
        direction = Vector2.right;
        inCombat = false;
        moveSpeed = 4f;
        damage = 20;
        stunned = false;
        enemyToEngage = null;
        fittedEnemyPosition = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (inCombat)
        {
            return;
        }
        Move();

        if (enemyToEngage == null)
        {
            DetectEnemy();
        }

        if (enemyToEngage != null && !fittedEnemyPosition)
        {
            EngageEnemy();
        }
        else if (enemyToEngage != null && fittedEnemyPosition)
        {
            if (Vector3.Distance(this.transform.position, enemyToEngage.transform.position) < 1.6f)
            {
                WarriorBattle battle = new WarriorBattle(this, enemyToEngage);
                if (enemyToEngage.inCombat) battle.Run1SidedBattle();
                else battle.RunBattle();
            }
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

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        fittedEnemyPosition = true;
        direction = Vector2.right;
    }
}

