using System;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class CastManager
    {
        public static CastManager Instance;

        private Dictionary<KeyCode, Spell> _spells = new();
        private Dictionary<Spell, float> _spellsLastCastTime = new();

        public CastManager()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            LoadSpells();
        }

        public void StartCd()
        {
            foreach (var spell in _spells.Values)
            {
                _spellsLastCastTime[spell] = Time.time;
                EventManager.Instance.InvokeEvent(EventNames.OnSpellCast, spell);
            }
        }

        public void TryToCastSpell(KeyCode keyCode, Quaternion rotation)
        {   
            bool casted = CastSpellIfReady(keyCode, rotation);
            if (casted)
            {
                EventManager.Instance.InvokeEvent(EventNames.OnSpellCast, _spells[keyCode]);
            }
        }

        private void LoadSpells()
        {
            var villageWarriorSpell = Resources.Load<Spell>("VillageWarriorSpell");
            AddSpell(villageWarriorSpell);
            var fairyDustSpell = Resources.Load<Spell>("FairyDustArrow");
            AddSpell(fairyDustSpell);
            var divineArrow = Resources.Load<Spell>("DivineArrow");
            AddSpell(divineArrow);
        }

        private void AddSpell(Spell spell)
        {
            if (spell != null)
            {
                _spells.Add(spell.GetKeyCode(), spell);
                spell.Init();
            }
        }

        private bool CastSpellIfReady(KeyCode keyCode, Quaternion rotation)
        {
            if (_spells.TryGetValue(keyCode, out Spell spell))
            {
                float cooldown = spell.GetCooldown();
                if (Time.time - _spellsLastCastTime[spell] >= cooldown)
                {
                    Vector3 mousePos = 
                        MainCamera.Instance.MatchMouseCoordinatesToCamera(InputManager.Instance.GetMousePosition());
                    Vector3 startingPosition = GetSpellStartingPosition(spell, mousePos);
                 
                    spell.Cast(mousePos, startingPosition, rotation);
                    _spellsLastCastTime[spell] = Time.time;
                    return true;
                }
            }
            return false;
        }

        private Vector3 GetSpellStartingPosition(Spell spell, Vector3 mousePos)
        {
            if (spell is BasicArrowSpell arrow)
            {
                return Constants.BowPosition;
            }
            else  // is a warrior
            {
                return new Vector3(Constants.BowPosition.x, mousePos.y, 0);
            }
        }
    }
}
