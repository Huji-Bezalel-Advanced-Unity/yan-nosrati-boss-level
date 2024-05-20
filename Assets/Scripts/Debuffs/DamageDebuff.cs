using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDebuff : Debuff
{
    public int Damage { get; private set; }

    public DamageDebuff(int damage)
    {
        Damage = damage;
    }

    public void Apply(Entity entity)
    {
        entity.RemoveHealth(Damage);
    }
}
