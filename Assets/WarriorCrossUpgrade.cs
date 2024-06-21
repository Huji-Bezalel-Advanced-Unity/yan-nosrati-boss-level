using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorCrossUpgrade : MonoBehaviour
{
    public static Action OnWarriorCross;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Warrior>())
        {
            OnWarriorCross?.Invoke();
        }
    }
}
