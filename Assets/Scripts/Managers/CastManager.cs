using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public abstract class CastManager
{
    
    protected Dictionary<Spell, float> _spellCooldowns = new Dictionary<Spell, float>();

    public CastManager(List<Spell> spellsList)
    {
        InitiallizeData(spellsList);
    }
    private void InitiallizeData(List<Spell> spells)
    {
        foreach (var spell in spells)
        {
            _spellCooldowns[spell] = spell.GetCooldown();
        }
    }

    public void UpdateSpellsCooldowns()
    {
        List<Spell> keys = new List<Spell>(_spellCooldowns.Keys);

        // Iterate over the list of keys and update the values in the dictionary
        foreach (var key in keys)
        {
            _spellCooldowns[key] = Mathf.Max(0, _spellCooldowns[key] - Time.deltaTime);
        }
    }
}
