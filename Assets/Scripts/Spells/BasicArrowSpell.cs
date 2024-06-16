using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class BasicArrowSpell : Spell
{
    protected Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        DebuffsList = new List<Debuff> {new DamageDebuff(10)};
    }

    

    public void Update()
    {
        if (transform.position.x > Constants.OutOfBoundsXPos || transform.position.y > Constants.OutOfBoundsYPos ||
            transform.position.y < -Constants.OutOfBoundsYPos)
        {
            Destroy(this.gameObject);
        }
    }


    public override void Cast(Vector2 direction,Vector3 startingPosition, Quaternion playerRotation)
    {
        // BasicArrowSpell arrow  = Instantiate(this, Constants.BowPosition, playerRotation);
        // // Convert screen coordinates to world coordinates
        // Vector3 pos = MainCamera.Camera.MatchMouseCoordinatesToCamera(direction);
        // print($"{pos} {startingPosition} ");
        // // I did this calculation myself and chatgpt couldn't come up with it, im proud
        // Vector2 d = (new Vector2(pos.x, pos.y) + (new Vector2(startingPosition.x,startingPosition.y)*-1)).normalized;
        // print($" {d}");
        // print("---------");

        // arrow.rb.AddForce(d * moveSpeed, ForceMode2D.Impulse);
        BasicArrowSpell arrow  = Instantiate(this, Constants.BowPosition, playerRotation);
        // Convert screen coordinates to world coordinates
        // I did this calculation myself and chatgpt couldn't come up with it, im proud
        // Vector2 d = (direction + (new Vector2(startingPosition.x,startingPosition.y)*-1)).normalized;
        Vector2 d = (direction +(Vector2) Constants.BowPosition*-1).normalized;
        arrow.rb.AddForce(d * moveSpeed, ForceMode2D.Impulse);
    }
}