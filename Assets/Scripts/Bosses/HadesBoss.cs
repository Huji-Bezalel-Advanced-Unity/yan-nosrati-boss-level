using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.MovementStrategies;
using Managers;
using Spells;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Warriors;
using Random = UnityEngine.Random;

namespace Bosses
{
    public class HadesBoss : Boss
    {
        private float _timeToChangeDirection;
        private Rigidbody2D _rb;
        private float _outOfBoundsTimer;
        [SerializeField] private RockThrowSpell rockThrowSpell; // will active only in boss Phase.LowHealth
        [SerializeField] private Renderer _renderer; // will active only in boss Phase.LowHealth

        void Awake()
        {
            direction = Vector2.up;
            _rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            _timeToChangeDirection = Random.value * 3;
            _outOfBoundsTimer = 0.5f;
            currentPhase = Phase.HighHealth;
            LowHealthSpells = new List<Spell>() { rockThrowSpell };
            _movementStrategy = new LinearMovementStrategy();


        }

        private async void ChangeVisibility(float start, float end, float duration)
        {
            await Util.DoFadeLerp(_renderer, start, end, duration);
        }

        public override void Init(Transform healthBarUI)
        {
            healthBar = healthBarUI;
            spellsCaster = Instantiate(spellsCaster, Vector3.zero,Quaternion.identity);
            spellsCaster.Init();
            ChangeVisibility(1, 0, 10);
            StartCoroutine(SelfUpdate());

        }

        // this is like a clock that is ticking every 1 second and casting spells that are ready.
        private IEnumerator SelfUpdate()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                List<Spell> spellsToCast = spellsCaster.CastSpellsOffCooldown();
                foreach (var spell in spellsToCast)
                {
                    spell.Cast(Vector2.left, GetSummonPosition(), Quaternion.identity);
                    StartCoroutine(RevealShortly());
                }
            }
        }

        void Update()
        {
            if (stunned) return;
            HandleDirectionChange();
            Move();
            _outOfBoundsTimer = Mathf.Max(0, _outOfBoundsTimer - Time.deltaTime);
        }

        private IEnumerator RevealShortly()
        {
            float duration = (Random.value + 1);
            ChangeVisibility(_renderer.material.color.a, 1, duration);
            yield return new WaitForSeconds(duration);
            ChangeVisibility(1, 0, duration);


        }

        private Vector3 GetSummonPosition()
        {
            Vector3 defaultPos = transform.position + Vector3.left * 1.2f;
            const float almostOutOfBoundsPos = 8f;
            if (transform.position.y < -almostOutOfBoundsPos)
            {
                defaultPos += Vector3.up * 2;
            }
            else if (transform.position.y > almostOutOfBoundsPos)
            {
                defaultPos += Vector3.down * 2;

            }

            return defaultPos;
        }

        private void HandleDirectionChange()
        {
            _timeToChangeDirection = Mathf.Max(0, _timeToChangeDirection - Time.deltaTime);
            if (_outOfBoundsTimer > 0) return;
            bool outOfBounds = BossOutOfBounds();
            if (_timeToChangeDirection == 0 || outOfBounds)
            {
                direction *= -1;
                _timeToChangeDirection = 1 + Random.value * 3;
                if (outOfBounds) _outOfBoundsTimer = 0.2f;
            }
        }

        private bool BossOutOfBounds()
        {
            return transform.position.y < -Constants.OutOfBoundsYPos ||
                   transform.position.y > Constants.OutOfBoundsYPos;
        }



        public override void Move()
        {
            _movementStrategy.Move(this, _rb, transform, direction, moveSpeed);
        }




        private void OnTriggerEnter2D(Collider2D col)
        {
            Spell spellHit = col.gameObject.GetComponent<Spell>();
            if (spellHit != null) spellHit.ApllySpellDebuffs(this);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.GetComponent<Spell>() != null)
            {
                Spell spell = collision.gameObject.GetComponent<Spell>();
                if (spell) spell.ApllySpellDebuffs(this);
            }
            else if (collision.gameObject.GetComponent<VillageWarrior>())
            {
                VillageWarrior warrior = collision.gameObject.GetComponent<VillageWarrior>();
                RemoveHealth(warrior.damage);
                Destroy(warrior.gameObject);
                // create explosion
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
            moveSpeed = moveSpeed * (1 + precentChange);
            spellsCaster.ChangeSpellsCooldown(1 - precentChange);
            if (currentPhase == Phase.LowHealth)
            {
                spellsCaster.AddSpell(rockThrowSpell);
                _movementStrategy = new CircularMovementStrategy();
            }
        }



    }
}
