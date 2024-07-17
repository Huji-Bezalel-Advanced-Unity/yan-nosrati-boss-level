using System;
using System.Collections;
using System.Collections.Generic;
using Bosses;
using DefaultNamespace.MovementStrategies;
using Managers;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Update = Unity.VisualScripting.Update;

public abstract class Boss : Entity
{
    protected internal Vector2 direction;
    [SerializeField] protected SpellsCaster spellsCaster;
    protected MovementStrategy MovementStrategy;
    protected List<Spell> lowHealthSpells;


    public abstract void Init(Transform healthBarUI);

    protected override IEnumerator Die()
    {
        animator.SetTrigger("Die");
        GameManager.Instance.WinGame();
        yield return null;
        // end game with win
    }
    
    
}




