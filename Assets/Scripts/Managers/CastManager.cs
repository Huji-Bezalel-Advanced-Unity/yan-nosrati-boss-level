using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class CastManager
{
    private BasicArrowSpell _basicArrowSpell;
    private SummonVillageWarriorSpell _summonVillageWarriorSpell;
    private Dictionary<Spell, float> _spellCooldowns = new Dictionary<Spell, float>();
    private Dictionary<KeyCode, Spell> _keyCodeToSpell = new Dictionary<KeyCode,Spell>();

    

    public CastManager(BasicArrowSpell basicArrowSpell, SummonVillageWarriorSpell summonVillageWarriorSpell)
    {
        _basicArrowSpell = basicArrowSpell;
        _summonVillageWarriorSpell = summonVillageWarriorSpell;
        InitiallizeData();
    }

 

    private void InitiallizeData()
    {
        _spellCooldowns[_basicArrowSpell] = _basicArrowSpell.GetCooldown();
        _spellCooldowns[_summonVillageWarriorSpell] = _summonVillageWarriorSpell.GetCooldown();
        _keyCodeToSpell[KeyCode.Q] = _summonVillageWarriorSpell;
    }

    // Update is called once per frame
    

    public void TryToShootBasicArrow(Quaternion rotation)
    {
        if (_spellCooldowns[_basicArrowSpell] == 0)
        {
            
            _basicArrowSpell.Cast(InputManager.Instance.GetMousePosition(), rotation);
            _spellCooldowns[_basicArrowSpell] = _basicArrowSpell.GetCooldown();
        }
    }

    

    public void TryToCastSpell(KeyCode keyCode, Quaternion rotation)
    {
        Spell spell = _keyCodeToSpell[keyCode];
        if (_spellCooldowns[spell] == 0)
        { 
            spell.Cast(InputManager.Instance.GetMousePosition(), rotation);
            _spellCooldowns[spell] = spell.GetCooldown();
        }
        else
        {
            Debug.Log("COOLDDOWN!!");
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
