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

        private Dictionary<KeyCode, ValueTuple<Spell, float>> _spells =
            new();

        private Dictionary<Spell, float> _spellsLastCastTime = new();

        public CastManager()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            LoadSpells();
        }


        public void StartCD()
        {
            foreach (var kvp in _spells)
            {
                EventManager.Instance.InvokeEvent(EventNames.OnSpellCast, kvp.Value.Item1);
            }
        }

        public void TryToCastSpell(KeyCode keyCode, Vector3 startingPosition, Quaternion rotation)
        {
            bool casted = CastSpellIfReady(keyCode, rotation, startingPosition);
            if (casted)
            {
                EventManager.Instance.InvokeEvent(EventNames.OnSpellCast, _spells[keyCode].Item1);
            }
        }

        private void LoadSpells()
        {
            var _villageWarriorSpell = Resources.Load<Spell>("VillageWarriorSpell");
            AddSpell(_villageWarriorSpell);
            var _fairyDustSpell = Resources.Load<Spell>("FairyDustArrow");
            AddSpell(_fairyDustSpell);
            var _divineArrow = Resources.Load<Spell>("DivineArrow");
            AddSpell(_divineArrow);
        }

        private void AddSpell(Spell spell)
        {
            if (spell != null)
            {
                _spells.Add(spell.GetKeyCode(), (spell, spell.GetCooldown()));
                _spellsLastCastTime[spell] = Time.time;
                spell.Init();
            }
        }

        private bool CastSpellIfReady(KeyCode keyCode, Quaternion rotation, Vector3 startingPosition)
        {
            var (spell, cooldown) = _spells[keyCode];

            if (Time.time - _spellsLastCastTime[spell] >= cooldown)
            {
                Vector3 mousePos =
                    MainCamera.Instance.MatchMouseCoordinatesToCamera(InputManager.Instance.GetMousePosition());
                spell.Cast(mousePos, startingPosition, rotation);
                _spellsLastCastTime[spell] = Time.time;
                return true;
            }

            return false;
        }
    }
}