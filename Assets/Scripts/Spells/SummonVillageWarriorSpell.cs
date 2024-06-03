using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonVillageWarriorSpell : Spell
{
    [SerializeField] private Warrior warriorToSummon;

    private void Awake()
    {
        coolDown = 0f;
        DebuffsList = new List<Debuff>{};
    }

    public override void Cast(Vector2 direction,Vector3 startingPosition, Quaternion rotation)
    {
        Instantiate(warriorToSummon, startingPosition, Quaternion.identity);
        
    }
}
