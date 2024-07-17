﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Debuffs
{
    public class InstantKillDebuff : Debuff
    {
        private int maxDamage = 10000;  // istant kill
        private int enemyLayer = 6;

        public void Apply(Entity entity)
        {
            if (entity.gameObject.layer == enemyLayer) entity.ChangeHealth(maxDamage);
        }
    }
}