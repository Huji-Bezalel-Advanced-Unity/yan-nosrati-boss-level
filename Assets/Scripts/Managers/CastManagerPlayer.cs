using System;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class CastManagerPlayer : CastManager
    {
        private Dictionary<KeyCode, Spell> _keyCodeToSpell = new Dictionary<KeyCode,Spell>();
        private BasicArrowSpell _basicArrowSpell;
        private CastManagerPlayerUI _castManagerPlayerUI;

        public CastManagerPlayer(List<Spell> spellsList, Dictionary<Spell, ValueTuple<Image, TextMeshProUGUI>> uiElements ): base(spellsList)
        {
            _castManagerPlayerUI = new CastManagerPlayerUI(uiElements);
            InitializeKeysToSpellsDict(spellsList);
            foreach (var spell in spellsList)
            {
                if (spell.CompareTag("BasicArrow"))
                {
                    _basicArrowSpell = (BasicArrowSpell)spell;
                    continue; //we continue since the basic arrow has no UI component
                }
                _castManagerPlayerUI.DisplaySpellCd(spell);
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
                    TutorialManager.Instance.RunSpellTutorial(GetKeyFromMap(spell),_castManagerPlayerUI.GetUIPosition(spell));
                    spell.SetFirstCast();
                }
                _castManagerPlayerUI.DisplaySpellCd(spell);
            }
        }

        private KeyCode GetKeyFromMap(Spell spell)
        {
            foreach (var (keyCode,spell_) in _keyCodeToSpell)
            {
                if (spell_ == spell) return keyCode;
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
}

