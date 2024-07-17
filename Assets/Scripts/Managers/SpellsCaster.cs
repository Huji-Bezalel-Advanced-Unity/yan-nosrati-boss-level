using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Managers
{
    public class SpellsCaster : MonoBehaviour
    {

        [SerializeField] private List<Spell> spells;
        
        private Dictionary<Spell,int> _spellCooldowns;
        
        public void Init()
        {
            _spellCooldowns = new Dictionary<Spell, int>();
            foreach (var spell in spells)
            {
                spell.Init();
                _spellCooldowns[spell] = (int)spell.GetCooldown();
            }

            StartCoroutine(SelfUpdate());
        }

        private IEnumerator SelfUpdate()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
            
                foreach (Spell val in _spellCooldowns.Keys.ToList())
                {
                    _spellCooldowns[val] = Mathf.Max(0, _spellCooldowns[val] - 1);
                }
            }
        }

        public List<Spell> CastSpellsOffCooldown()
        {
            
            List<Spell> spellsToCast = new List<Spell>();
            foreach (var keyValuePair in _spellCooldowns.ToList())
            {
                if (keyValuePair.Value <= 0)
                {
                    spellsToCast.Add(keyValuePair.Key);
                    _spellCooldowns[keyValuePair.Key] = (int)keyValuePair.Key.GetCooldown();
                }
            }
            return spellsToCast;
        }

        public void AddSpell(Spell spell)
        {
            _spellCooldowns[spell] = (int)spell.GetCooldown();
        }


        public void ChangeSpellsCooldown(float precentChange)
        {
            foreach (var item in _spellCooldowns)
            {
                item.Key.setCooldown((int)item.Key.GetCooldown()*precentChange);
            }
        }
    }
}
