using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HadesBoss : Boss
{
    private float _timeToChangeDirection;
    private Rigidbody2D rb;
    private float outOfBoundsTimer;
    void Awake()
    {
        direction = Vector2.up;
        spells = new Dictionary<Spell, float>() { };
        rb = GetComponent<Rigidbody2D>();
        _timeToChangeDirection = Random.value * 3;
        moveSpeed = 5f;
        outOfBoundsTimer = 0.5f;
        health = 100;
        maxHealth = 100;


    }

    // Update is called once per frame
    void Update()
    {
        HandleDirectionChange();
        Move();
        outOfBoundsTimer = Mathf.Max(0, outOfBoundsTimer - Time.deltaTime);

    }

    private void HandleDirectionChange()
    {
        _timeToChangeDirection = Mathf.Max(0, _timeToChangeDirection - Time.deltaTime);
        if (outOfBoundsTimer > 0) return;
        bool outOfBounds = BossOutOfBounds();
        if (_timeToChangeDirection == 0 || outOfBounds)
        {
            direction *= -1;
            _timeToChangeDirection = 1 + Random.value * 3;
            if (outOfBounds) outOfBoundsTimer = 0.2f;
        }
    }

    private bool BossOutOfBounds()
    {
        return transform.position.y < -Constants.OutOfBoundsYPos || transform.position.y > Constants.OutOfBoundsYPos;
    }


    public override void Move()
    {
 
      
        rb.MovePosition((Vector2)transform.position + direction*Mathf.Sin(Time.deltaTime*moveSpeed)*1.4f);
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        Spell spellHit = col.gameObject.GetComponent<Spell>();
        if (spellHit != null)
        {
            foreach (Debuff debuff in spellHit.GetSpellsDebuffs())
            {
                debuff.Apply(this);
            }
        }
    }
  
    
    
}
