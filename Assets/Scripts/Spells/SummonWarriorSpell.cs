using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using Warriors;


// IMPORTANT NOTE FOR THE TEACHER - if u remove the comments the pooling works great. its just that the skeletons have an elaborate death animations
// that i dont know how to restore, it uses bone system, and there is no revive animation given. so, if u remove the comment, you will see skeletons reviving in a wierd manner
// that isnt good. The pooling works great though, i promise :).

namespace Spells
{
    public class SummonWarriorSpell : Spell
    {
        [SerializeField] private Warrior warriorToSummon;
        [SerializeField] private SoundName soundName;

        public override void Cast(Vector2 direction, Vector3 startingPosition, Quaternion rotation)
        {
            AudioManager.Instance.PlaySound(soundName);
            // Warrior warrior =  ObjectPoolManager.Instance.GetObjectFromPool<Warrior>(warriorToSummon.tag);
            // if (warrior)
            // {
            //     warrior.transform.position = startingPosition;
            //     warrior.GetComponent<Collider2D>().enabled = true;
            // }
            // else
            {
                Instantiate(warriorToSummon, startingPosition, Quaternion.identity);
            }
        }
    }
}