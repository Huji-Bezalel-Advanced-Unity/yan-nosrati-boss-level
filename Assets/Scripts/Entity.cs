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
    public GameObject healthBar;
    public bool isDead;
    public Animator animator;


    
    public void RemoveHealth(int damage)
    {
        health = Mathf.Max(0, health - damage);
        float nextScale = (float) health / maxHealth;
        healthBar.transform.localScale =
            new Vector3(nextScale, healthBar.transform.localScale.y, 0);

        if (health == 0)
        {
            isDead = true;
            healthBar.gameObject.SetActive(false);
        }
    }

    // public IEnumerator Die()
    // {
    //     isDead = true;
    //     animator.SetTrigger("Die");
    //     yield return new WaitForSeconds(2f);
    //     Destroy(this.gameObject);
    //
    // }
    
        
    

    public abstract void Move();
}
