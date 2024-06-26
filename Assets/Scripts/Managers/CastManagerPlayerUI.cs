using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DefaultNamespace;
using Spells;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class CastManagerPlayerUI 
    {
        private Dictionary<Spell, ValueTuple<Image, TextMeshProUGUI>> _uiElements;
        public CastManagerPlayerUI(Dictionary<Spell, ValueTuple<Image, TextMeshProUGUI>> UIElements)
        {
            _uiElements = UIElements;
        }

        public void DisplaySpellCd(Spell spell)
        {
            _uiElements[spell].Item1.fillAmount = 0.5f;
            Util.DoFillLerp(_uiElements[spell].Item1, 1, 0, spell.GetCooldown());
            StartCountdown(_uiElements[spell].Item2, spell.GetCooldown());
        }

        public Vector3 GetUIPosition(Spell spell)
        {
            return _uiElements[spell].Item1.transform.position;
        }

        private async void StartCountdown(TextMeshProUGUI t, float countFrom)
        {
            t.gameObject.SetActive(true);
            float timer = countFrom;
            while (timer > 0)
            {
                t.text = ((int)timer).ToString();
                await Task.Delay(1000);
                timer -= 1;
            }
            
            if (t)  t.gameObject.SetActive(false);  // i had to add if for somer eason
        }
        
    }
}