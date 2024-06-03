
   using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
   

public class CastManagerPlayer : CastManager
{
  

    private Dictionary<KeyCode, Spell> _keyCodeToSpell = new Dictionary<KeyCode,Spell>();
    private BasicArrowSpell _basicArrowSpell;

    public CastManagerPlayer(List<Spell> spellsList): base(spellsList)
    {
        InitializeKeysToSpellsDict(spellsList);
        foreach (var spell in spellsList)
        {
            if (spell.GetComponent<BasicArrowSpell>() != null)
            {
                _basicArrowSpell = (BasicArrowSpell)spell;
            }
        }
    }

    private void InitializeKeysToSpellsDict(List<Spell> spellsList)
    {
        _keyCodeToSpell[KeyCode.Q] = spellsList[0];
        
    }
  
    // Update is called once per frame
    

    public void TryToShootBasicArrow(Quaternion rotation, Vector3 startingPosition)
    {
        if (_spellCooldowns[_basicArrowSpell] == 0)
        {
            
            _basicArrowSpell.Cast(InputManager.Instance.GetMousePosition(),startingPosition, rotation);
            _spellCooldowns[_basicArrowSpell] = _basicArrowSpell.GetCooldown();
        }
    }
    

    public void  TryToCastSpell(KeyCode keyCode,Vector3 startingPosition, Quaternion rotation)
    {
        Spell spell = _keyCodeToSpell[keyCode];
        if (_spellCooldowns[spell] == 0)
        { 
            spell.Cast(InputManager.Instance.GetMousePosition(),startingPosition, rotation);
            _spellCooldowns[spell] = spell.GetCooldown();
        }
        else
        {
            Debug.Log("COOLDDOWN!!");
        }
    }

   
    
}

