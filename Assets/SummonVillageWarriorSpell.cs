using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonVillageWarriorSpell : Spell
{
    [SerializeField] private Warrior warriorToSummon;

    private void Awake()
    {
        coolDown = 6f;
        DebuffsList = new List<Debuff> { new DamageDebuff(15) };
    }

    public override void Cast(Vector2 direction)
    {
        Vector3 mousePos = MainCamera.Camera.MatchMouseCoordinatesToCamera(direction);
        Instantiate(warriorToSummon, new Vector3(Constants.BowPosition.x, mousePos.y, 0f), Quaternion.identity);
        
    }
}
