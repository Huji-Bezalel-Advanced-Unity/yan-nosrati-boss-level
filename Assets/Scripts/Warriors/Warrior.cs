using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Warrior : Entity
{
    [SerializeField] private Rigidbody2D rb;
    public int damage;
    public float attackTime;
    public Vector2 direction;
    public bool inCombat;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    private void Start()
    {
        animator.SetBool("Move", true);
    }

    public override void  Move()
    {
        if (inCombat) return;
        rb.MovePosition((Vector2)transform.position + direction*moveSpeed*Time.deltaTime);
    }

    public void EnterBattle()
    {
        print(gameObject.name);
        inCombat = true;
        animator.SetBool("Move", false);
        animator.SetBool("InCombat", true);
    }

    public void ExitBattle()
    {
        if (isDead)
        {
            StartCoroutine(Die());
            return;
        }
        inCombat = false;
        animator.SetBool("Move", true);
        animator.SetBool("InCombat", false);
    }

    private IEnumerator Die()
    {
        animator.SetTrigger("Die");
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
        yield return null;
    }
}