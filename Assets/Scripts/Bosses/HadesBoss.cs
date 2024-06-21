
using System;
using Bosses;
using Spells;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.MovementStrategies;
using Random = UnityEngine.Random;

public class HadesBoss : Boss
{
    private float _timeToChangeDirection;
    private Rigidbody2D rb;
    private float outOfBoundsTimer;
    [SerializeField] private RockThrowSpell rockThrowSpell;
    //REMVove
    private float angle;

    // [SerializeField] private SkeletonWarrior _warrior;

     // [SerializeField] private SummonSkeletonWarriorSpell _skeletonWarriorSpell;

   
    void Awake()
    {
        direction = Vector2.up;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        _timeToChangeDirection = Random.value * 3;
        outOfBoundsTimer = 0.5f;
        health = 100;
        maxHealth = 100;
        currentPhase = Phase.HighHealth;
        LowHealthSpells = new List<Spell>() { rockThrowSpell };
        _movementStrategy = new LinearMovementStrategy();
    

    }

    private void Start()
    {
        // GoInvisible();
       
    }

    private async void GoInvisible()
    {
        var renderer = GetComponent<Renderer>();
        await Util.DoFadeLerp(renderer, 1, 0,10f);
    }

    public override void Init(CastManagerBoss castManagerBoss, Transform healthBarUI)
    {
        castManager = castManagerBoss;
        healthBar = healthBarUI;

    }
    void Update()
    {
        if (stunned) return;
        HandleDirectionChange();
        Move();
        outOfBoundsTimer = Mathf.Max(0, outOfBoundsTimer - Time.deltaTime);
        Spell casted = castManager.TryToCastSpell(Vector2.left, GetSummonPosition(), Quaternion.identity);
        // if (casted is not null) animator.SetTrigger(casted.tag); 
    }

    private Vector3 GetSummonPosition()
    {
        Vector3 defaultPos = transform.position + Vector3.left * 1.2f;
        float _AlmostOutOfBoundsPos = 8f;
        if (transform.position.y < -_AlmostOutOfBoundsPos)
        {
            defaultPos += Vector3.up*2;
        }
        else if (transform.position.y > _AlmostOutOfBoundsPos)
        {
            defaultPos += Vector3.down*2;

        }
        return defaultPos;
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


    protected override IEnumerator Die()
    {
        yield return null;
    }

    public override void Move()
    {
        _movementStrategy.Move(this, rb,transform,direction,moveSpeed);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Spell>() != null)
        {
            Spell spell = collision.gameObject.GetComponent<Spell>();
            if (spell) spell.ApllySpellDebuffs(this);
          
        }
    }

    public override void RemoveHealth(int damage)
    {
        base.RemoveHealth(damage);
        if (currentPhase == Phase.HighHealth && health <= maxHealth / 2)
        {
            currentPhase = Phase.MediumHealth;
            ChangePhase();
        }
        else if (currentPhase == Phase.MediumHealth && health <= maxHealth / 4)
        {
            currentPhase = Phase.LowHealth;
            ChangePhase();
        }
    }

    private void ChangePhase()
    {
        float precentChange = 0.2f;
        moveSpeed = moveSpeed * (1+precentChange);
        castManager.ChangeSpellsCooldown(1-precentChange);
        if (currentPhase == Phase.LowHealth)
        {
            castManager.AddSpell(rockThrowSpell);
            _movementStrategy = new CircularMovementStrategy();
        }
    }
}
