
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class CastManagerBoss : CastManager
{
    public CastManagerBoss(List<Spell> spellsList) : base(spellsList){}

    public Spell TryToCastSpell(Vector2 direction, Vector3 startingPosition, Quaternion rotation)
    {
        Spell spellCasted = null;
        foreach (var (spell,cd) in _spellCooldowns)
        {
            if ((DateTime.UtcNow - cd).TotalSeconds > spell.GetCooldown())
            {  
                spellCasted = spell;
                spell.Cast(direction,startingPosition,rotation);
                break;
            }
        }

        if (spellCasted is not null)
        {
            Debug.Log(spellCasted.GetCooldown());
            _spellCooldowns[spellCasted] = DateTime.UtcNow;
        }
        return spellCasted;

    }
    
 
    
    
    
}
