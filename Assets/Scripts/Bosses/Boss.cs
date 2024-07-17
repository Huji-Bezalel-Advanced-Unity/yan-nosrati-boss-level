using System;
using System.Collections;
using System.Collections.Generic;
using Bosses;
using MovementStrategies;
using Managers;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Update = Unity.VisualScripting.Update;

public abstract class Boss : Entity
{
    [SerializeField] protected SpellsCaster spellsCaster;
    
    protected Vector2 direction;
    protected MovementStrategy MovementStrategy;
    protected List<Spell> LowHealthSpells;


    public abstract void Init(Transform healthBarUI);

    protected override IEnumerator Die()
    {
        animator.SetTrigger("Die");
        GameManager.Instance.WinGame();
        yield return null;
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;
    }
    
    
}




