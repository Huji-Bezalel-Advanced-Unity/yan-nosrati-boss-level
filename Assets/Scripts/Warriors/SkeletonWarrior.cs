using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonWarrior : Warrior
{
    void Awake()
    {
        health = 1000;
        maxHealth = 100;
        attackTime = 1f;
        direction = Vector2.left;
        inCombat = false;
        moveSpeed = 3f;
        damage = 20;
        stunned = false;
    
    }

    // Update is called once per frame
    void Update()
    {
        if (inCombat) return;
        Move();
        
    }

    public void Battle()
    {
        inCombat = true;
        animator.SetBool("Move", false);
        animator.SetBool("InCombat", true);
    }

    public void ExitBattle()
    {
        inCombat = false;
        animator.SetBool("Move", true);
        animator.SetBool("InCombat", false);
    }
}
