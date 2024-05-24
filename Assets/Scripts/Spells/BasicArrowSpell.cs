using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class BasicArrowSpell : Spell
{
    private Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        DebuffsList = new List<Debuff> { new DamageDebuff(10) };
        moveSpeed = 20f;
        coolDown = 1f;

    }

    

    private void Update()
    {
        if (transform.position.x > Constants.OutOfBoundsXPos || transform.position.y > Constants.OutOfBoundsYPos ||
            transform.position.y < -Constants.OutOfBoundsYPos)
        {
            print(transform.position);
            Destroy(this.gameObject);
        }
    }


    public override void Cast(Vector2 direction, Quaternion playerRotation)
    {
        
        BasicArrowSpell arrow  = Instantiate(this, Constants.BowPosition, playerRotation);
        // Convert screen coordinates to world coordinates
        print("REACHED");
        Vector3 pos = MainCamera.Camera.MatchMouseCoordinatesToCamera(direction);

        print("REACHED");
        // I did this calculation myself and chatgpt couldn't come up with it, im proud
        direction = (new Vector2(pos.x, pos.y) + (new Vector2(Constants.BowPosition.x, Constants.BowPosition.y)*-1)).normalized;

        arrow.rb.AddForce(direction * moveSpeed, ForceMode2D.Impulse);
    }
}