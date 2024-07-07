using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Warriors;

public class SummonWarriorSpell : Spell
{
    [SerializeField] private Warrior warriorToSummon;
    public override void Cast(Vector2 direction,Vector3 startingPosition, Quaternion rotation)
    {
       Warrior warrior =  ObjectPoolManager.Instance.GetWarriorFromPool(warriorToSummon.tag);
       if (warrior)
       {
           warrior.transform.position = startingPosition;
       }
       else
       {
           Instantiate(warriorToSummon, startingPosition, Quaternion.identity);
       }
    }
}
