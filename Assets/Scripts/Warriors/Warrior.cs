using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Managers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Warrior : Entity
{
    [SerializeField] private Rigidbody2D rb;
    public int damage;
    public bool inCombat = false;
    public Vector2 baseDirection;
    public Vector2 curDirection;
    public Queue<Warrior> inCombatWith;
    public bool fittedEnemyPosition;
    public DamageFlash damageFlash;


    private void Awake()
    {
        inCombatWith = new Queue<Warrior>();
        rb = GetComponent<Rigidbody2D>();
        fittedEnemyPosition = false; // why???????? shuld bef ailse by default
        damageFlash = GetComponent<DamageFlash>();
    }

    public void FixedUpdate()
    {
        if (inCombatWith.Count > 0 || isDead) return;
        Move();
    }

    private void OnEnable()
    {
        animator.Play("start");
    }

    private void OnDisable()
    {
        animator.SetBool("Move", false);
    }

    public override void Move()
    {
        rb.MovePosition((Vector2)transform.position + curDirection * moveSpeed * Time.deltaTime);
    }

    public void EnterBattle(Warrior warriorToBattle)
    {
        if (inCombatWith.Contains(warriorToBattle) || warriorToBattle.isDead) return;
        inCombatWith.Enqueue(warriorToBattle);
        inCombat = true;
        Fight();
    }

    public void Fight()
    {
        animator.SetBool("Move", false);
        animator.SetBool("InCombat", true);
    }

    public virtual void ExitBattle()
    {
        if (inCombatWith.Count > 0)
        {
            inCombatWith.Dequeue();
        }

        if (inCombatWith.Count == 0)
        {
            inCombat = false;
            animator.SetBool("InCombat", false);
            animator.SetBool("Move", true);
        }
    }

    public virtual void ResetWarrior()
    {
        inCombatWith.Clear();
        inCombat = false;
        curDirection = baseDirection;
        healthBar.parent.gameObject.SetActive(true);
        isDead = false;
        ChangeHealth(-(maxHealth - health)); //restore healthbar
        gameObject.SetActive(false);
    }

    public void AttackAnimationTriggered()
    {
        if (inCombatWith.Count == 0 || inCombatWith.Peek().isDead) return;
        inCombatWith.Peek().damageFlash.CallFlasher();
        DamageEnemy(inCombatWith.Peek(), damage);
    }

    protected override IEnumerator Die()
    {
        isDead = true;
        gameObject.GetComponent<Collider2D>().enabled = false;
        animator.SetTrigger("Die");
        foreach (var warrior in inCombatWith)
        {
            warrior.ExitBattle();
        }
        inCombatWith.Clear();
        inCombat = false;
        healthBar.parent.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f); // let the body lay on the ground for 2 seconds
        ObjectPoolManager.Instance.AddObjectToPool(this);
    }
}