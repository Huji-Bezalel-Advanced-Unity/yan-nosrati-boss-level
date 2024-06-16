using System;
using System.Collections;
using System.Collections.Generic;
using Bosses;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Update = Unity.VisualScripting.Update;

public abstract class Boss : Entity
{
    protected Vector2 direction;
    protected CastManagerBoss castManager;
    protected Phase currentPhase = Phase.HighHealth;
    protected List<Spell> LowHealthSpells;


    public abstract void Init(CastManagerBoss castManagerBoss, Transform healthBarUI);

    protected override IEnumerator Die()
    {
        animator.SetTrigger("Die");
        yield return null;
        // end game
    }
    
    
}


