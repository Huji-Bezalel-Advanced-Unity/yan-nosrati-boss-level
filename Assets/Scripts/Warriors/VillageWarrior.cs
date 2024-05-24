using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageWarrior : Warrior
{
    // Start is called before the first frame update
    void Awake()
    {
        health = 200;
        maxHealth = 200;
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
        if (Input.GetKeyDown(KeyCode.A))
        {
            print("CALLLED -25");
            RemoveHealth(25);
            
        }
    }
    
    

  
    
}
