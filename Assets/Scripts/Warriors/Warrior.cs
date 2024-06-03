using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Warrior : Entity
{
    [SerializeField] private Rigidbody2D rb;
    public int damage;
    public float attackTime;
    public Vector2 baseDirection;
    public Vector2 curDirection;
    public Warrior inCombatWith = null;
    public Warrior enemyToEngage = null;
    public bool fittedEnemyPosition = false;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }
    
    public void FixedUpdate()
    {
        if (inCombatWith)
        {
            if (inCombatWith.isDead)
            {
                inCombatWith.ExitBattle();
                ExitBattle();
            }
        }

        if (enemyToEngage == null)
        {
            DetectEnemy();
        }

        if (enemyToEngage != null && !enemyToEngage.isDead && !fittedEnemyPosition)
        {
            EngageEnemy();
        }
        else if (enemyToEngage != null && fittedEnemyPosition)
        {
            if (Vector3.Distance(this.transform.position, enemyToEngage.transform.position) < 1.6f)
            {
                // WarriorBattle battle = new WarriorBattle(this, enemyToEngage);
                // else battle.RunBattle();
                EnterBattle(enemyToEngage);
            }
        }
        Move();


        

    }
    private void DetectEnemy()
    
    {
        // print(gameObject.tag);
        Collider2D[] overlappingObjects =  Physics2D.OverlapCircleAll(transform.position, 7f);
        foreach (Collider2D col in overlappingObjects)
        {
            if (col.gameObject.tag.Contains("Warrior") && !col.gameObject.CompareTag(gameObject.tag))
            {
                // print(gameObject.tag);
                // print(col.gameObject.tag);
                Warrior potentialEnemy = col.gameObject.GetComponent<Warrior>();
                if (!potentialEnemy.isDead)
                {
                    enemyToEngage = potentialEnemy;
                    return;
                }
            }
        }
    }

    private void EngageEnemy()
    {
        if (enemyToEngage.isDead) // still buggy when warrior goes to attack something that will die before he reaches it
        {
            enemyToEngage = null;
            curDirection = baseDirection;
        }
        if (enemyToEngage.transform.position.y > 0.07f + transform.position.y)
        {
            curDirection =  Vector2.up;
        }
        else if (enemyToEngage.transform.position.y +0.07f < transform.position.y)
        {
            curDirection = Vector2.down;
        }
        else
        {
            fittedEnemyPosition = true;
            curDirection = baseDirection;
        }
    }
    private void Start()
    {
        animator.SetBool("Move", true);
    }

    public override void  Move()
    {
        if (inCombatWith || isDead ) return;
        rb.MovePosition((Vector2)transform.position + curDirection*moveSpeed*Time.deltaTime);
    }

    public void EnterBattle(Warrior warriorToBattle)
    {
        inCombatWith = warriorToBattle;
        animator.SetBool("Move", false);
        animator.SetBool("InCombat", true);
    }

    public void ExitBattle()
    {
        inCombatWith = null;
        enemyToEngage = null;
        fittedEnemyPosition = false;
        if (isDead)
        {
            
            animator.SetTrigger("Die");
            StartCoroutine(Die());
            return;
        }
        animator.SetBool("InCombat", false);
        animator.SetBool("Move", true);
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
        yield return null;
    }
    
    // will be triggered by attack animations
    
}