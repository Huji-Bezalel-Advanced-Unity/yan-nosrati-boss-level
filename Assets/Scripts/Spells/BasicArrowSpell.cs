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
            ObjectPoolManager.Instance.AddSpellToPool(this);
        }
    }

    public override void Cast(Vector2 direction, Vector3 startingPosition, Quaternion playerRotation)
    {
        Spell spell = GetSpellFromPool(direction, startingPosition, playerRotation, gameObject.tag);
        BasicArrowSpell arrowSpell= (BasicArrowSpell)spell;
        spell.transform.rotation = playerRotation;
        Vector2 d = (direction +(Vector2) Constants.BowPosition*-1).normalized;
        arrowSpell.rb.AddForce(d * moveSpeed, ForceMode2D.Impulse);
    }

    // Generic method to handle common casting logic
    private void OnCollisionEnter2D(Collision2D other)
    {
        gameObject.SetActive(false);
        ObjectPoolManager.Instance.AddSpellToPool(this);
    }

  
}