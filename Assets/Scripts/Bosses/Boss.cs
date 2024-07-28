using System.Collections;
using System.Collections.Generic;
using MovementStrategies;
using Managers;
using UnityEngine;

namespace Bosses
{
    public abstract class Boss : Entity
    {
        [SerializeField] protected SpellsCaster spellsCaster;

        protected MovementStrategy MovementStrategy;
        protected List<Spell> LowHealthSpells;
        
        public abstract void Init(Transform healthBarUI);

        protected override IEnumerator Die()
        {
            animator.SetTrigger("Die");
            GameManager.Instance.WinGame();
            yield return null;
        }
        
    }
}