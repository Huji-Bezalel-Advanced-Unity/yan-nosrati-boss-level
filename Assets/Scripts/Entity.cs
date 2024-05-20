using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public int health;
    public bool stunned;


    public void RemoveHealth(int damage)
    {
        health -= damage;
    }
}
