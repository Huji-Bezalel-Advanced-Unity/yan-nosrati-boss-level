// using System;
// using System.Collections.Generic;
// using System.Diagnostics;
// using DefaultNamespace;
// using TMPro;
// using Unity.VisualScripting;
// using UnityEngine;
// using UnityEngine.UI;
//
// namespace Managers
// {
//     public class CastManager
//     {
//         public static CastManager Instance;
//         private Dictionary<KeyCode, ValueTuple<Spell,float>> _spells = new Dictionary<KeyCode, ValueTuple<Spell,float>>();
//         private Dictionary<Spell, Stopwatch> _spellsCooldowns = new Dictionary<Spell, Stopwatch>();
//
//
//         public CastManager()
//         {
//             if (Instance == null)
//             {
//                 Instance = this;
//             }
//             LoadSpells();
//         }
//
//         private void LoadSpells()
//         {
//             var _villageWarriorSpell = Resources.Load<Spell>("VillageWarriorSpell");
//             _spells.Add(_villageWarriorSpell.GetKeyCode(),(_villageWarriorSpell,_villageWarriorSpell.GetCooldown()));
//             var _fairyDustSpell = Resources.Load<Spell>("FairyDustArrow");
//             _spells.Add(_fairyDustSpell.GetKeyCode(),(_fairyDustSpell,_fairyDustSpell.GetCooldown()));
//             var _divineArrow = Resources.Load<Spell>("DivineArrow");
//             _spells.Add(_divineArrow.GetKeyCode(),(_divineArrow,_divineArrow.GetCooldown()));
//         }
//
//
//   
//         
//         public void TryToCastSpell(KeyCode keyCode,Vector3 startingPosition, Quaternion rotation)
//         {
//             bool casted = CastSpellIfReady(keyCode,rotation,startingPosition); 
//             
//             // need to cast event when casted !
//             
//             // if (casted)
//             // {
//             //     if (GameManager.Instance.GetRunTutorial() && spell.GetFirstCast())
//             //     {
//             //         // TutorialManager.Instance.RunSpellTutorial(GetKeyFromMap(spell),_castManagerPlayerUI.GetUIPosition(spell));
//             //         spell.SetFirstCast();
//             //     }
//             //     _castManagerPlayerUI.DisplaySpellCd(spell);
//             // }
//         }
//
//         // private KeyCode GetKeyFromMap(Spell spell)
//         // {
//         //     foreach (var (keyCode,spell_) in spells)
//         //     {
//         //         if (spell_ == spell) return keyCode;
//         //     }
//         //     return KeyCode.None;
//         // }
//
//         private bool CastSpellIfReady(KeyCode keycode, Quaternion rotation, Vector3 startingPosition)
//         {
//             if (_spellsCooldowns[_spells[keycode].Item1].Elapsed.TotalSeconds >= _spells[keycode].Item2)
//             {
//                 Vector3 mousePos = MainCamera.Instance.MatchMouseCoordinatesToCamera(InputManager.Instance.GetMousePosition());
//                 _spells[keycode].Item1.Cast(mousePos,startingPosition, rotation);
//                 _spellsCooldowns[_spells[keycode].Item1].Restart();
//                 return true;
//             }
//
//             return false;
//         }
//
//    
//     
//     }
// }
//
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
        private Dictionary<KeyCode, ValueTuple<Spell, float>> _spells = new Dictionary<KeyCode, ValueTuple<Spell, float>>();
        private Dictionary<Spell, float> _spellsLastCastTime = new Dictionary<Spell, float>();

        public CastManager()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            LoadSpells();
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

        public void TryToCastSpell(KeyCode keyCode, Vector3 startingPosition, Quaternion rotation)
        {
            bool casted = CastSpellIfReady(keyCode, rotation, startingPosition);

            // need to cast event when casted !
            // if (casted)
            // {
            //     if (GameManager.Instance.GetRunTutorial() && spell.GetFirstCast())
            //     {
            //         // TutorialManager.Instance.RunSpellTutorial(GetKeyFromMap(spell),_castManagerPlayerUI.GetUIPosition(spell));
            //         spell.SetFirstCast();
            //     }
            //     _castManagerPlayerUI.DisplaySpellCd(spell);
            // }
        }

        private bool CastSpellIfReady(KeyCode keyCode, Quaternion rotation, Vector3 startingPosition)
        {
            var (spell, cooldown) = _spells[keyCode];
        
            if (Time.time - _spellsLastCastTime[spell] >= cooldown)
            {
                Vector3 mousePos = MainCamera.Instance.MatchMouseCoordinatesToCamera(InputManager.Instance.GetMousePosition());
                spell.Cast(mousePos , startingPosition, rotation);
                _spellsLastCastTime[spell] = Time.time;
                return true;
            }

            return false;
        }
    }
}
