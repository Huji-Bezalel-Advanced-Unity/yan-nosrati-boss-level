using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spells
{
    public class SummonSkeletonWarriorSpell : Spell
    {
        [SerializeField] private Warrior warriorToSummon;
        private void Awake()
        {
            coolDown = 10f;
            DebuffsList = new List<Debuff>{};
        }

        public override void Cast(Vector2 direction,Vector3 startingPosition, Quaternion rotation)
        {
            Vector3 mousePos = MainCamera.Camera.MatchMouseCoordinatesToCamera(direction);
            Instantiate(warriorToSummon, startingPosition, Quaternion.identity);
        
        }
    }

}