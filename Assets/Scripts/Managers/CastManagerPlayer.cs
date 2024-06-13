
   using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using Spells;
using TMPro;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class CastManagerPlayer : CastManager
{
  

    private Dictionary<KeyCode, Spell> _keyCodeToSpell = new Dictionary<KeyCode,Spell>();
    private BasicArrowSpell _basicArrowSpell;
    private FairyDustSpell _fairyDustSpell;
    private CastManagerPlayerUI castManagerPlayerUI;

    public CastManagerPlayer(List<Spell> spellsList, Dictionary<Spell, ValueTuple<Image, TextMeshProUGUI>> UIElements ): base(spellsList)
    {
         castManagerPlayerUI = new CastManagerPlayerUI(UIElements);
        InitializeKeysToSpellsDict(spellsList);
        foreach (var spell in spellsList)
        {
            if (spell.GetComponent<BasicArrowSpell>() != null)
            {
                _basicArrowSpell = (BasicArrowSpell)spell;
                continue; //we continue since the basic arrow is attached o a UI component
            }
            castManagerPlayerUI.DisplaySpellCD(spell);

        }
    }

    private void InitializeKeysToSpellsDict(List<Spell> spellsList)
    {
        _keyCodeToSpell[KeyCode.Q] = spellsList[0];
        _keyCodeToSpell[KeyCode.W] = spellsList[1];

    }
  
    

    public void TryToShootBasicArrow(Quaternion rotation, Vector3 startingPosition)
    {
        if (_spellCooldowns[_basicArrowSpell] == 0)
        {
            
            _basicArrowSpell.Cast((Vector2)MainCamera.Camera.MatchMouseCoordinatesToCamera(InputManager.Instance.GetMousePosition())
                ,startingPosition, rotation);
            _spellCooldowns[_basicArrowSpell] = _basicArrowSpell.GetCooldown();
        }
    }
    

    public void  TryToCastSpell(KeyCode keyCode,Vector3 startingPosition, Quaternion rotation)
    {
        
        Vector3 mousePos = MainCamera.Camera.MatchMouseCoordinatesToCamera(InputManager.Instance.GetMousePosition());

        Spell spell = _keyCodeToSpell[keyCode];
        if (_spellCooldowns[spell] == 0)
        { 
            spell.Cast(mousePos,startingPosition, rotation);
            _spellCooldowns[spell] = spell.GetCooldown();
            castManagerPlayerUI.DisplaySpellCD(spell);
        }
        else
        {
            Debug.Log("COOLDDOWN!!");
        }
    }

   
    
}

