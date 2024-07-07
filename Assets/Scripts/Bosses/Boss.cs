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
    public Vector2 direction;
    [SerializeField] public SpellsCaster spellsCaster;
    public MovementStrategy MovementStrategy;
    public List<Spell> lowHealthSpells;


    public abstract void Init(Transform healthBarUI);

    protected override IEnumerator Die()
    {
        animator.SetTrigger("Die");
        yield return null;
        // end game with win
    }
    
    
}




