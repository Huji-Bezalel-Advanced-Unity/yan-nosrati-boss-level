using System.Collections;
using System.Collections.Generic;
using Bosses;
using UnityEngine;
using UnityEngine.UI;

public abstract class Entity : MonoBehaviour
{
    protected Phase currentPhase = Phase.HighHealth;
    public int health;
    public int maxHealth;
    public bool stunned;
    public float moveSpeed;
    public Transform healthBar;
    public bool isDead;
    public Animator animator;
    public Renderer renderer;

    protected abstract IEnumerator Die();
    public abstract void Move();
    
    public virtual void ChangeHealth(Entity unitTakingDamage, int damage)
    {
        if (isDead) return;
        unitTakingDamage.health = Mathf.Max(0, unitTakingDamage.health - damage);
        float nextScale = (float)unitTakingDamage.health / unitTakingDamage.maxHealth;
        unitTakingDamage.healthBar.transform.localScale =
            new Vector3(nextScale, healthBar.transform.localScale.y, 0);

        if (unitTakingDamage.health == 0)
        {
            StartCoroutine(unitTakingDamage.Die());
        }
    }
}