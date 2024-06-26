using System.Collections.Generic;
using Debuffs;
using UnityEngine;

namespace Spells
{
    public class DivineArrowSpell: BasicArrowSpell
    {
        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            DebuffsList = new List<Debuff> { new RevealDebuff(4), new SlowDebuff(30, 5),
                new DamageDebuff(50), new StunDebuff(3f), new InstantKillDebuff()};
        }
        private new void Update()
        {
            base.Update();
        }
        
    }
}