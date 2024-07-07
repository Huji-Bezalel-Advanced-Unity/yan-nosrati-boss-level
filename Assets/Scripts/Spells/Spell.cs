using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Spell : MonoBehaviour
{
    protected List<Debuff> DebuffsList;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float baseCoolDown;
    [SerializeField] protected float coolDown;
    [SerializeField] private KeyCode keyCode;
    protected bool FirstCast;

    
    public void Init()
    {
        coolDown = baseCoolDown;
        FirstCast = true;
    }

    public KeyCode GetKeyCode()
    {
        return keyCode;
    }

    public abstract void Cast(Vector2 direction, Vector3 startingPosition, Quaternion playerRotation);

    public Spell GetSpellFromPool(Vector2 direction, Vector3 startingPosition, Quaternion playerRotation,string tag)
    {
        Spell spell = ObjectPoolManager.Instance.GetSpellFromPool(tag);
        if (spell)
        {
            spell.gameObject.SetActive(true);
            spell.transform.position = startingPosition; 
        }
        else
        {
            spell = Instantiate(this, startingPosition, playerRotation);
        }
        return spell;

    }


    public void ApllySpellDebuffs(Entity entity)
    {
     
        foreach (Debuff debuff in DebuffsList)
        {
            debuff.Apply(entity);
        }
    }
    public List<Debuff> GetSpellsDebuffs()
    {
        return DebuffsList;
    }

    public float GetCooldown()
    {
        return coolDown;
    }

    public void setCooldown(float newCooldown)
    {
        coolDown = newCooldown;
    }

    public void SetFirstCast()
    {
        FirstCast = false;
    }

    public bool GetFirstCast()
    {
        return FirstCast;
    }
}