using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using MovementStrategies;
using Managers;
using Spells;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities;
using Warriors;
using Random = UnityEngine.Random;

namespace Bosses
{
    public class HadesBoss : Boss
    {
        [SerializeField] private RockThrowSpell rockThrowSpell; // will active only in boss Phase.LowHealth

        private Rigidbody2D _rb;
        private float _timeToChangeDirection;
        private float minTimeToSwtichDirection = 0.35f;  // this timer helps with boss getting stuck on the edge of the screen 
        private float _summonOffset = 1.2f;
        private float mediumHealth;
        private float lowHealth;
        private Vector2 direction;
        

        public override void Init(Transform healthBarUI)
        {
            healthBar = healthBarUI;

            spellsCaster = Instantiate(spellsCaster, Vector3.zero, Quaternion.identity);
            spellsCaster.Init();

            direction = Vector2.up;
            _rb = GetComponent<Rigidbody2D>();
            _timeToChangeDirection = Random.value * 3;
            mediumHealth = maxHealth * 2 / 3f;
            lowHealth =  maxHealth * 1 / 3f;

            LowHealthSpells = new List<Spell>() { rockThrowSpell };
            MovementStrategy = new LinearMovementStrategy();

            ChangeVisibility(1, 0, 10);
            StartCoroutine(SpellUpdate());
            StartCoroutine(SelfUpdate());
        }

        private void ChangeVisibility(float start, float end, float duration)
        {
            StartCoroutine(Util.DoFadeLerp(renderer, start, end, duration, null));
        }


        // this is like a clock that is ticking every 1 second and casting spells that are ready.
        private IEnumerator SpellUpdate()
        {
            while (!isDead)
            {
                yield return new WaitForSeconds(1f);
                List<Spell> spellsToCast = spellsCaster.CastSpellsOffCooldown();
                foreach (var spell in spellsToCast)
                {
                    spell.Cast(Vector2.left, transform.position + Vector3.left * _summonOffset, Quaternion.identity);
                    StartCoroutine(RevealShortly());
                }
            }
        }

        private IEnumerator SelfUpdate()
        {
            while (!isDead)
            {
                while (stunned)
                {
                    yield return null;
                }
                HandleDirectionChange();
                Move();
                minTimeToSwtichDirection = Mathf.Max(0, minTimeToSwtichDirection - Time.deltaTime);
                yield return null;
            }
        }

        private IEnumerator RevealShortly()
        {
            float duration = 0.25f;
            ChangeVisibility(renderer.material.color.a, 1, duration);
            yield return new WaitForSeconds(duration);
            ChangeVisibility(1, 0, duration);
        }

        private void HandleDirectionChange()
        {
            _timeToChangeDirection = Mathf.Max(0, _timeToChangeDirection - Time.deltaTime);
            if (minTimeToSwtichDirection > 0) return;

            bool outOfBounds = IsBossOutOfBounds();

            if (_timeToChangeDirection == 0 || outOfBounds)
            {
                direction *= -1;
                _timeToChangeDirection = 1 + Random.value * 3;
                if (outOfBounds) minTimeToSwtichDirection = 0.35f;
            }
        }

        private bool IsBossOutOfBounds()
        {
            return transform.position.y < -Constants.OutOfBoundsYPos ||
                   transform.position.y > Constants.OutOfBoundsYPos;
        }


        public override void Move()
        {
            MovementStrategy.Move(this, _rb, transform, direction, moveSpeed);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            ApplySpellIfSpellHit(col.gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (ApplySpellIfSpellHit(collision.gameObject))
            {
                return;
            }

            if (collision.gameObject.GetComponent<VillageWarrior>())
            {
                VillageWarrior warrior = collision.gameObject.GetComponent<VillageWarrior>();
                ChangeHealth(this, warrior.GetDamage());
                ObjectPoolManager.Instance.AddObjectToPool(warrior);
            }
        }

        private bool ApplySpellIfSpellHit(GameObject col)
        {
            Spell spell = col.GetComponent<Spell>();
            if (spell != null)
            {
                spell.ApllySpellDebuffs(this);
                AudioManager.Instance.PlaySound(spell.hitSound);
                return true;
            }

            return false;
        }

        public override void ChangeHealth(Entity unitTakingDamage, int damage)
        {
            base.ChangeHealth(this, damage);
            EvaluateHealthPhase();
        }

        private void EvaluateHealthPhase()
        {
            if (currentPhase == Phase.HighHealth && health <= mediumHealth)
            {
                currentPhase = Phase.MediumHealth;
                ChangePhase();
            }
            else if (currentPhase == Phase.MediumHealth && health <= lowHealth)
            {
                currentPhase = Phase.LowHealth;
                ChangePhase();
            }
        }
        
        private void ChangePhase()
        {
            const float percentChange = 0.2f;
            moveSpeed *= (1 + percentChange);
            spellsCaster.ChangeSpellsCooldown(1 - percentChange);

            if (currentPhase == Phase.LowHealth)
            {
                spellsCaster.AddSpell(rockThrowSpell);
                MovementStrategy = new CircularMovementStrategy();
            }
        }
    }
}