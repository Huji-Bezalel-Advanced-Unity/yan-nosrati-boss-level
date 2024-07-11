using System;
using System.Collections;
using Bosses;
using Managers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Warriors;

namespace Warriors
{
    public class Player : Entity
    {
        public static Action<Phase> OnPlayerChangePhase;
        public static Action<Phase> OnTakingDamage;
        private float currentAngle;
        [SerializeField] private GameObject bow;
        [SerializeField] private Spell basicArrow;

        public void Init(Transform healthBarUI)
        {
            healthBar = healthBarUI;
            StartCoroutine(SelfUpdate());
            StartCoroutine(ShootArrow());
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
            WarriorCrossUpgrade.OnWarriorCross += UpgradeBow;  // this ok??

        }
        private void OnDisable()
        {
            InputManager.KeyPressed -= ReactToKeyPress;
            WarriorCrossUpgrade.OnWarriorCross -= UpgradeBow;  // this ok??

        }

        protected override IEnumerator Die()
        {
            yield return null;
        }

        public override void Move()
        {
            // Get the mouse position in world coordinates
            Vector3 mousePosition = MainCamera.Instance.MatchMouseCoordinatesToCamera(Input.mousePosition);

            // Calculate the direction from the sprite to the mouse position
            Vector3 direction = mousePosition - transform.position;
            // Calculate the angle between the sprite's current direction and the target direction
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Clamp the angle between -60 and 60 degrees
            float clampedAngle = Mathf.Clamp(targetAngle, -65f, 65f);

            // Smoothly interpolate towards the clamped angle to retain smooth movement
            currentAngle = Mathf.LerpAngle(currentAngle, clampedAngle, Time.deltaTime * 10f);

            // Apply the rotation to the sprite
            bow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, currentAngle));
        }

        private void ReactToKeyPress(KeyCode key)
        {
            Vector3 mousePos =
                MainCamera.Instance.MatchMouseCoordinatesToCamera(InputManager.Instance.GetMousePosition());
            CastManager.Instance.TryToCastSpell(key,
                new Vector3(Constants.BowPosition.x, mousePos.y, 0),
                bow.transform.rotation);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            SkeletonWarrior skeletonWarrior = col.gameObject.GetComponent<SkeletonWarrior>();
            if (skeletonWarrior)
            {
                RemoveHealth(skeletonWarrior.damage);
                Destroy(skeletonWarrior.gameObject);
            }

            Spell spell = col.gameObject.GetComponent<Spell>();
            if (spell)
            {
                print(spell.name);
                spell.ApllySpellDebuffs(this);
                Destroy(spell.gameObject);
            }
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            Spell spell = other.gameObject.GetComponent<Spell>();
            if (spell)
            {
                spell.ApllySpellDebuffs(this);
                Destroy(spell.gameObject);
            }
        }

        public override void RemoveHealth(int damage)
        {
            base.RemoveHealth(damage);
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