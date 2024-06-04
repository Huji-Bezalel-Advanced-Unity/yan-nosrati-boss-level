﻿using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SlowDebuff : Debuff
{
    public int Slow { get; private set; }
    public float Duration;

    public SlowDebuff(int slowPrecent, float duration)
    {
        Slow = slowPrecent;
        Duration = duration;
    }

    public void Apply(Entity entity)
    {
        DoSlowForSeconds(entity);
    }

    private async Task DoSlowForSeconds(Entity entity)
    {
        float speed = entity.moveSpeed;
        entity.moveSpeed = (float)(100 - Slow)/100 * entity.moveSpeed;
        await Task.Delay((int)Duration*1000);
        entity.moveSpeed = speed;
        Debug.Log(entity.moveSpeed);
    }
}