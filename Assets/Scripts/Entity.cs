using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Entity : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public bool stunned;
    public float moveSpeed;
    public Image healthBar;
    public bool isDead;
    public Animator animator;


    
    // public void RemoveHealth(int damage)
    // {
    //     health = Mathf.Max(0, health - damage);
    //     float nextScale = (float) health / maxHealth;
    //     healthBar.transform.localScale =
    //         new Vector3(nextScale, healthBar.transform.localScale.y, 0);
    //
    //     if (health == 0)
    //     {
    //         isDead = true;
    //         healthBar.gameObject.SetActive(false);
    //     }
    // }
    public void RemoveHealth(int damage)
    {
        health = Mathf.Max(0, health - damage);
        healthBar.fillAmount = (float)health / maxHealth;
        if (health == 0)
        {
            StartCoroutine(Die());
            print("dead");
        }
    }

    protected abstract IEnumerator Die();

    //
    // public void DamageEnemy(Entity unitTakingDamage, int damage)
    // {
    //     unitTakingDamage.health = Mathf.Max(0, unitTakingDamage.health - damage);
    //     float nextScale = (float) unitTakingDamage.health / unitTakingDamage.maxHealth;
    //     unitTakingDamage.healthBar.transform.localScale =
    //         new Vector3(nextScale, healthBar.transform.localScale.y, 0);
    //
    //     if (unitTakingDamage.health == 0)
    //     {
    //         unitTakingDamage.isDead = true;
    //         unitTakingDamage.healthBar.gameObject.SetActive(false);
    //     }
    // }
    public void DamageEnemy(Entity unitTakingDamage, int damage)
    {
        unitTakingDamage.health = Mathf.Max(0, unitTakingDamage.health - damage);
        healthBar.fillAmount = (float)health / maxHealth;
        if (unitTakingDamage.health == 0)
        {
            StartCoroutine(unitTakingDamage.Die());
            print("dead");
        }
    }

  
    
        
    

     public abstract void Move();
}
