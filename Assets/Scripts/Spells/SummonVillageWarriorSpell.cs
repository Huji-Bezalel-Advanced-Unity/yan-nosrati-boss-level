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

    public override void Cast(Vector2 direction, Quaternion rotation)
    {
        Vector3 mousePos = MainCamera.Camera.MatchMouseCoordinatesToCamera(direction);
        Instantiate(warriorToSummon, new Vector3(Constants.BowPosition.x, mousePos.y, 0f), Quaternion.identity);
        
    }
}
