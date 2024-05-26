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

    public override void Move()
    {   
        if (inCombat) return;
        rb.MovePosition((Vector2)transform.position + direction*moveSpeed*Time.deltaTime);
    }

    public void ReachEnd() { Destroy(this.gameObject);}


}
