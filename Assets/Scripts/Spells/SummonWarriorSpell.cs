using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonWarriorSpell : Spell
{
    [SerializeField] private Warrior warriorToSummon;

  

    public override void Cast(Vector2 direction,Vector3 startingPosition, Quaternion rotation)
    {
        Instantiate(warriorToSummon, startingPosition, Quaternion.identity);
        
    }
}
