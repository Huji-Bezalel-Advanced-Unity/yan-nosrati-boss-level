using System.Collections.Generic;
using UnityEngine;


namespace Spells
{

public class FairyDustSpell : BasicArrowSpell
{
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        DebuffsList = new List<Debuff> { new RevealDebuff(4), new SlowDebuff(25, 5), new DamageDebuff(10)};
    }

    private new void Update()
    {
        base.Update();
    }
    
}
}