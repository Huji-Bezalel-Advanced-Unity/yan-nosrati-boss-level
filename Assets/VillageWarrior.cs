using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageWarrior : Warrior
{
    // Start is called before the first frame update
    void Awake()
    {
        health = 100;
        attackTime = 2.5f;
        direction = Vector2.right;
        inCombat = false;
        moveSpeed = 4f;
        damage = 15;
        stunned = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

  
    
}
