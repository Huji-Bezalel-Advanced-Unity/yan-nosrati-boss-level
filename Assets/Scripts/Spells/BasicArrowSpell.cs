using System;
using System.Collections.Generic;
using Managers;
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
        if (IsOutOfBounds())
        {
            ObjectPoolManager.Instance.AddObjectToPool(this);
        }
    }

    private bool IsOutOfBounds()
    {
        return transform.position.x > Constants.MaxCameraX || transform.position.y > Constants.MaxCameraY ||
               transform.position.x <= Constants.MinCameraX ||transform.position.y < Constants.MinCameraY;
    }

    public override void Cast(Vector2 direction, Vector3 startingPosition, Quaternion playerRotation)
    {
        Spell spell = GetSpellFromPool(direction, startingPosition, playerRotation, gameObject.tag);
        BasicArrowSpell arrowSpell= (BasicArrowSpell)spell;
        spell.transform.rotation = playerRotation;
        Vector2 d = (direction +(Vector2) Constants.BowPosition*-1).normalized;
        arrowSpell.rb.AddForce(d * moveSpeed, ForceMode2D.Impulse);
    }

    public new void ResetSpell()
    {
        base.ResetSpell();
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    // Generic method to handle common casting logic
    private void OnCollisionEnter2D(Collision2D other)
    {
        ObjectPoolManager.Instance.AddObjectToPool(this);
    }

  
}