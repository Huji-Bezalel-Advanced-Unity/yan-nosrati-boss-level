using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Spells
{
    public class SummonSkeletonWarriorSpell : Spell
    {
        [SerializeField] private Warrior warriorToSummon;
        
        public override void Cast(Vector2 direction,Vector3 startingPosition, Quaternion rotation)
        {
            Instantiate(warriorToSummon, startingPosition, Quaternion.identity);
        }

     
    }

}