using System;
using System.Collections;
using Bosses;
using Managers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Warriors;

namespace player
{
    public class Player : Entity
    {
        public static Action<Phase> OnPlayerChangePhase;
        public static Action<Phase> OnTakingDamage;
        
        [SerializeField] private GameObject bow;
        [SerializeField] private Spell basicArrow;
        
        private float currentAngle;
    

        public void Init(Transform healthBarUI)
        {
            healthBar = healthBarUI;
            animator = GetComponent<Animator>();
            
            StartCoroutine(SelfUpdate());
            StartCoroutine(ShootArrow());
            
            basicArrow.Init();
        }

        private IEnumerator ShootArrow()
        {
            while (!isDead)
            {
                yield return new WaitForSeconds(basicArrow.GetCooldown());
                while (stunned) yield return null;  // Wait until not stunned
                Vector3 mousePos = MainCamera.Instance.MatchMouseCoordinatesToCamera(InputManager.Instance.GetMousePosition());
                basicArrow.Cast(mousePos, transform.position, bow.transform.rotation);
            }
        }

        private IEnumerator SelfUpdate()
        {
            while (!isDead)
            {
                while (stunned) yield return null;  // Wait until not stunned
                InputManager.Instance.CheckKeyPressed();
                Move();
                if (Input.GetKey(KeyCode.K))
                {
                    Die();
                }
                yield return null;
            }
        }

        private void UpgradeBow()
        {
            float reductionRate = 0.05f;
            float minSpellCooldown = 0.3f;
            basicArrow.setCooldown(Mathf.Max(basicArrow.GetCooldown() - reductionRate,minSpellCooldown));
        }

        private void OnEnable()
        {
            InputManager.KeyPressed += ReactToKeyPress;
            ObjectCrossMapTrigger.OnWarriorCross += UpgradeBow; 

        }
        private void OnDisable()
        {
            InputManager.KeyPressed -= ReactToKeyPress;
            ObjectCrossMapTrigger.OnWarriorCross -= UpgradeBow; 

        }

        protected override IEnumerator Die()
        {
            GameManager.Instance.LoseGame();
            yield return null;
        }

        public override void Move()
        {
            Vector3 mousePosition = MainCamera.Instance.MatchMouseCoordinatesToCamera(Input.mousePosition);
            
            Vector3 direction = mousePosition - transform.position;
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float clampedAngle = Mathf.Clamp(targetAngle, -65f, 65f);

            
            currentAngle = Mathf.LerpAngle(currentAngle, clampedAngle, Time.deltaTime * 10f);
            bow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, currentAngle));
        }

        private void ReactToKeyPress(KeyCode key)
        {
            CastManager.Instance.TryToCastSpell(key,
                transform.position,
                bow.transform.rotation);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            SkeletonWarrior skeletonWarrior = col.gameObject.GetComponent<SkeletonWarrior>();
            if (skeletonWarrior)
            {
                ChangeHealth(this, skeletonWarrior.damage);
                ObjectPoolManager.Instance.AddObjectToPool(skeletonWarrior);
                return;
            }

            Spell spell = col.gameObject.GetComponent<Spell>();
            if (spell)
            {
                print("trigger called");
                AudioManager.Instance.PlaySound(spell.hitSound);
                spell.ApllySpellDebuffs(this);
                ObjectPoolManager.Instance.AddObjectToPool(spell);
            }
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            Spell spell = other.gameObject.GetComponent<Spell>();
            if (spell)
            {
                print("collision called");
                AudioManager.Instance.PlaySound(spell.hitSound);
                spell.ApllySpellDebuffs(this);
                ObjectPoolManager.Instance.AddObjectToPool(spell);
            }
        }

        public override void ChangeHealth(Entity unitTakingDamage, int damage)
        {
            base.ChangeHealth(this, damage);
            if (currentPhase == Phase.HighHealth && health <= maxHealth / 2)
            {
                currentPhase = Phase.MediumHealth;
                OnPlayerChangePhase?.Invoke(currentPhase);
            }
            else if (currentPhase == Phase.MediumHealth && health <= maxHealth / 4)
            {
                currentPhase = Phase.LowHealth;
                OnPlayerChangePhase?.Invoke(currentPhase);
            }
            OnTakingDamage?.Invoke(currentPhase);

        }
    }
}