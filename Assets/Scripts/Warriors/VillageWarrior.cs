using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;

public class VillageWarrior : Warrior
{
    private Warrior _enemyToEngage;
    public void FixedUpdate()
    {
        base.FixedUpdate();
        if (!isDead) HandleEnemyInteraction();

    }
    
    private void HandleEnemyInteraction()
    {   
        if (_enemyToEngage == null)
        {
            DetectEnemy();
        }

        if (_enemyToEngage != null && !_enemyToEngage.isDead && !fittedEnemyPosition)
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
        if (_enemyToEngage.isDead || _enemyToEngage.transform.position.x +0.2f < transform.position.x) // still buggy when warrior goes to attack something that will die before he reaches it
        {
            _enemyToEngage = null;
            curDirection = baseDirection;
            print("get ficled");
            return;
        }
        if (_enemyToEngage.transform.position.y > 0.07f + transform.position.y)
        {
            curDirection =  Vector2.up;
        }
        else if (_enemyToEngage.transform.position.y +0.07f < transform.position.y)
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
        Collider2D[] overlappingObjects =  Physics2D.OverlapCircleAll(transform.position, 3f);
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
        return warrior &&!warrior.isDead &&
               !warrior.gameObject.CompareTag(gameObject.tag) &&
               transform.position.x < warrior.transform.position.x;

    }

    // private void OnCollisionEnter2D(Collision2D collision2D)
    // {
    //     print(collision2D.gameObject.tag);
    //     fittedEnemyPosition = true;
    //     curDirection = baseDirection;
    // }
    
    public override void ExitBattle()
    {
        base.ExitBattle();
        fittedEnemyPosition = false;
        inCombat = false;
        _enemyToEngage = null;
    }

    public void AttackAnimationTriggered()
    {
        if (inCombatWith.Count ==0 || inCombatWith.Peek().isDead) return;
        inCombatWith.Peek().damageFlash.CallFlasher();
        DamageEnemy(inCombatWith.Peek(),damage);
    }
}

