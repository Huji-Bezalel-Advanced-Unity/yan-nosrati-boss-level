using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class CastManagerPlayerUI
    {
        private Dictionary<Spell, ValueTuple<Image, TextMeshProUGUI>> _UIElements;

        public CastManagerPlayerUI(Dictionary<Spell, ValueTuple<Image, TextMeshProUGUI>> UIElements)
        {
            _UIElements = UIElements;
        }

        public void DisplaySpellCD(Spell spell)
        {
            _UIElements[spell].Item1.fillAmount = 0.5f;
            Util.DoFillLerp(_UIElements[spell].Item1, 1, 0, spell.GetCooldown());
            StartCountdown(_UIElements[spell].Item2, spell.GetCooldown());
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
            t.gameObject.SetActive(false);

            
        }
    }
}