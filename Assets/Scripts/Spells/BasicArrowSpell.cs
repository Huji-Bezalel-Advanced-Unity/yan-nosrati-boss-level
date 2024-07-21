using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Debuffs;
using Managers;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class BasicArrowSpell : Spell
{
    [SerializeField] private ParticleSystem particles;
    protected Rigidbody2D rb;


    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        DebuffsList = new List<Debuff> { new DamageDebuff(10) };
        
        particles = Instantiate(particles, Vector3.zero, Quaternion.identity);
        particles.gameObject.SetActive(false);
        
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
               transform.position.x <= Constants.MinCameraX || transform.position.y < Constants.MinCameraY;
    }

    public override void Cast(Vector2 direction, Vector3 startingPosition, Quaternion playerRotation)
    {
        Spell spell = GetSpellFromPool(startingPosition, playerRotation, gameObject.tag);
        BasicArrowSpell arrowSpell = (BasicArrowSpell)spell;

        spell.transform.rotation = playerRotation;
        Vector2 d = (direction + (Vector2)Constants.BowPosition * -1).normalized;

        arrowSpell.rb.AddForce(d * moveSpeed, ForceMode2D.Impulse);

        AudioManager.Instance.PlaySound(SoundName.ArrowShoot);
    }

    public override void ResetSpell()
    {
        base.ResetSpell();
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    private async void OnCollisionEnter2D(Collision2D other)
    {
        await Explode(other.GetContact(0).point);
        ObjectPoolManager.Instance.AddObjectToPool(this);
    }

    private async Task Explode(Vector3 position)
    {
        particles.gameObject.SetActive(true);
        particles.transform.position = position;
        particles.Play();

        // Convert particle duration to milliseconds and cast to int
        int durationInMilliseconds = (int)(particles.main.duration * 1000);
        await Task.Delay(durationInMilliseconds);

        particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        particles.gameObject.SetActive(false);
    }
}