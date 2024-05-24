using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    public List<Debuff> DebuffsList;
    public float moveSpeed;
    public float coolDown;

    public abstract void Cast(Vector2 direction, Quaternion PlayerRotation);

    public List<Debuff> GetSpellsDebuffs()
    {
        return DebuffsList;
    }

    public float GetCooldown()
    {
        return coolDown;
    }
}