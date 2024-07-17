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
        private Rigidbody2D _rb;
        private float _outOfBoundsTimer;
        private float _timeToChangeDirection;

        [SerializeField] private RockThrowSpell rockThrowSpell; // will active only in boss Phase.LowHealth

        [SerializeField] private float summonOffset = 1.2f;

        void Start()
        {
            direction = Vector2.up;
            _rb = GetComponent<Rigidbody2D>();
            _timeToChangeDirection = Random.value * 3;
            _outOfBoundsTimer = 0.5f;
            currentPhase = Phase.HighHealth;
            lowHealthSpells = new List<Spell>() { rockThrowSpell };
            MovementStrategy = new LinearMovementStrategy();
        }

        private async void ChangeVisibility(float start, float end, float duration)
        {
            await Util.DoFadeLerp(renderer, start, end, duration);
        }

        public override void Init(Transform healthBarUI)
        {
            healthBar = healthBarUI;
            spellsCaster = Instantiate(spellsCaster, Vector3.zero, Quaternion.identity);
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
                    spell.Cast(Vector2.left, transform.position + Vector3.left * summonOffset, Quaternion.identity);
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
            float duration = 0.25f;
            ChangeVisibility(renderer.material.color.a, 1, duration);
            yield return new WaitForSeconds(duration);
            ChangeVisibility(1, 0, duration);
        }

        // preventing warriors from spawning on UI
        // private Vector3 GetSummonPosition()
        // {
        //     float summonOffset = 1.2f;
        //     float cameraMaxY = MainCamera.Instance._camera.orthographicSize;
        //     float almostOutOfBoundsPos = cameraMaxY - summonOffset;
        //     Vector3 defaultPos = transform.position + Vector3.left * summonOffset;
        //     
        //     if (transform.position.y < -almostOutOfBoundsPos)
        //     {
        //         defaultPos += Vector3.up * 2;
        //     }
        //     else if (transform.position.y > almostOutOfBoundsPos)
        //     {
        //         defaultPos += Vector3.down * 2;
        //     }
        //
        //     return defaultPos;
        // }

        private void HandleDirectionChange()
        {
            _timeToChangeDirection = Mathf.Max(0, _timeToChangeDirection - Time.deltaTime);
            if (_outOfBoundsTimer > 0) return;
            bool outOfBounds = IsBossOutOfBounds();
            if (_timeToChangeDirection == 0 || outOfBounds)
            {
                direction *= -1;
                _timeToChangeDirection = 1 + Random.value * 3;
                if (outOfBounds) _outOfBoundsTimer = 0.2f;
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
                ChangeHealth(warrior.damage);
                Destroy(warrior.gameObject);
                // create explosion
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

        public override void ChangeHealth(int damage)
        {
            base.ChangeHealth(damage);
            print(maxHealth * 2f/3f);
            if (currentPhase == Phase.HighHealth && health <= maxHealth * (2f/3f))
            {
                currentPhase = Phase.MediumHealth;
                ChangePhase();
                print("chnge to medium");
            }
            else if (currentPhase == Phase.MediumHealth && health <= maxHealth * (1f/3f))
            {
                print("chnge to low");
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
                MovementStrategy = new CircularMovementStrategy();
            }
        }
    }
}