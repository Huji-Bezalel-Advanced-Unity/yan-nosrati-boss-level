using System;
using System.Collections;
using System.Collections.Generic;
using Bosses;
using DefaultNamespace.MovementStrategies;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Update = Unity.VisualScripting.Update;

public abstract class Boss : Entity
{
    public Vector2 direction;
    public CastManagerBoss castManager;
    public MovementStrategy _movementStrategy;
    public List<Spell> LowHealthSpells;


    public abstract void Init(CastManagerBoss castManagerBoss, Transform healthBarUI);

    protected override IEnumerator Die()
    {
        animator.SetTrigger("Die");
        yield return null;
        // end game
    }
    
    
}




