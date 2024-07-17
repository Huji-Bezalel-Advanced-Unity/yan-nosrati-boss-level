using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using Warriors;

public class SummonWarriorSpell : Spell
{
    [SerializeField] private Warrior warriorToSummon;
    [SerializeField] private SoundName soundName;
    public override void Cast(Vector2 direction,Vector3 startingPosition, Quaternion rotation)
    { 
        Vector3 summonPosition = new Vector3(startingPosition.x, direction.y, 0) +Vector3.right;
        AudioManager.Instance.PlaySound(soundName);
        Warrior warrior =  ObjectPoolManager.Instance.GetObjectFromPool<Warrior>(warriorToSummon.tag);
        if (warrior)
        {
            warrior.transform.position = summonPosition;
            warrior.GetComponent<Collider2D>().enabled = true;
        }
        else
        {
            Instantiate(warriorToSummon,summonPosition , Quaternion.identity);
        }
    }
    
}