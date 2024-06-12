
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

    public void  TryToCastSpell(Vector2 direction, Vector3 startingPosition, Quaternion rotation)
    {
        List<Spell> renewCooldown = new List<Spell>();
        foreach (var (spell,cd) in _spellCooldowns)
        {
            if (cd == 0)
            {
                spell.Cast(direction,startingPosition,rotation);
                renewCooldown.Add(spell);
            }
        }
        foreach (var spell in renewCooldown)
        {
            _spellCooldowns[spell] = spell.GetCooldown();
            Debug.Log($"cd: {spell.GetCooldown()}");
            
        }
    }
    
 
    
    
    
}
