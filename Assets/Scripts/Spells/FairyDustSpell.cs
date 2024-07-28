using System.Collections.Generic;
using Debuffs;
using UnityEngine;


namespace Spells
{

public class FairyDustSpell : BasicArrowSpell
{
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        DebuffsList = new List<Debuff> { new RevealDebuff(3), new SlowDebuff(50, 5), new DamageDebuff(10)};
    }

    private new void Update()
    {
        base.Update();
    }
    
}
}