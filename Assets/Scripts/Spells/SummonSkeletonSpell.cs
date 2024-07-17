using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using Warriors;

public class SummonSkeletonSpell : Spell
{
    [SerializeField] private Warrior warriorToSummon;
    [SerializeField] private SoundName soundName;
    public override void Cast(Vector2 direction,Vector3 startingPosition, Quaternion rotation)
    { 
        /// 
        ///TODO  add a revive animation because o.w i cant use the object pool.. i dont udnertsand animations...
        ///
        
        
       // Warrior warrior =  ObjectPoolManager.Instance.GetObjectFromPool<Warrior>(warriorToSummon.tag);
       // if (warrior)
       // {
       //     warrior.transform.position = startingPosition;
       //     warrior.GetComponent<Collider2D>().enabled = true;
       //
       // }
       // else
       {
           Instantiate(warriorToSummon, startingPosition, Quaternion.identity);
           AudioManager.Instance.PlaySound(soundName);
       }
    }

    public new void ResetSpell()
    {
        base.ResetSpell();
    }
}
