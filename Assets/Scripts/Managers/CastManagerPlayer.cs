
   using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
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
    private CastManagerPlayerUI castManagerPlayerUI;

    public CastManagerPlayer(List<Spell> spellsList, Dictionary<Spell, ValueTuple<Image, TextMeshProUGUI>> UIElements ): base(spellsList)
    {
        castManagerPlayerUI = new CastManagerPlayerUI(UIElements);
        InitializeKeysToSpellsDict(spellsList);
        foreach (var spell in spellsList)
        {
            if (spell.CompareTag("BasicArrow"))
            {
                _basicArrowSpell = (BasicArrowSpell)spell;
                continue; //we continue since the basic arrow has no UI component
            }
            castManagerPlayerUI.DisplaySpellCD(spell);
        }
        WarriorCrossUpgrade.OnWarriorCross += UpgradeBow;

    }

    private void UpgradeBow()
    {
        float reductionRate = 0.05f;
        _basicArrowSpell.setCooldown(_basicArrowSpell.GetCooldown() - reductionRate);
    }

    private void InitializeKeysToSpellsDict(List<Spell> spellsList)
    {
        _keyCodeToSpell[KeyCode.Q] = spellsList[0];
        _keyCodeToSpell[KeyCode.W] = spellsList[1];
        _keyCodeToSpell[KeyCode.R] = spellsList[2];

    }
    
    public void TryToShootBasicArrow(Quaternion rotation, Vector3 startingPosition)
    {
       CastSpellIfReady(_basicArrowSpell,rotation,startingPosition);
    }
    public void  TryToCastSpell(KeyCode keyCode,Vector3 startingPosition, Quaternion rotation)
    {
        Spell spell = _keyCodeToSpell[keyCode];
        bool casted = CastSpellIfReady(spell,rotation,startingPosition);
        if (casted)
        {
            if (GameManager.Instance.GetRunTutorial() && spell.GetFirstCast())
            {
                TutorialManager.Instance.RunSpellTutorial(GetKeyFromMap(spell),castManagerPlayerUI.GetUIPosition(spell));
                spell.SetFirstCast();
            }
            castManagerPlayerUI.DisplaySpellCD(spell);
        }
    }

    private KeyCode GetKeyFromMap(Spell spell)
    {
        foreach (var (KeyCode,_spell) in _keyCodeToSpell)
        {
            if (_spell == spell) return KeyCode;
        }
        return KeyCode.None;
    }

    private bool CastSpellIfReady(Spell spell, Quaternion rotation, Vector3 startingPosition)
    {
        if ((DateTime.UtcNow - _spellCooldowns[spell]).TotalSeconds < spell.GetCooldown()) return false;
        Vector3 mousePos = MainCamera.Instance.MatchMouseCoordinatesToCamera(InputManager.Instance.GetMousePosition());
        spell.Cast(mousePos,startingPosition, rotation);
        _spellCooldowns[spell] = DateTime.UtcNow;
        return true;

    }

   
    
}

