using System;
using System.Collections;
using System.Collections.Generic;
using Bosses;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;
using Update = Unity.VisualScripting.Update;

public abstract class Boss : Entity
{
    public Vector2 direction;
    public CastManagerBoss castManager;
    public Phase currentPhase = Phase.HighHealth;


    public abstract void Init(CastManagerBoss castManagerBoss);
}


