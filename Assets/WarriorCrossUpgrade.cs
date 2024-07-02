using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class WarriorCrossUpgrade : MonoBehaviour
{
    public static Action OnWarriorCross;
    [SerializeField] private ElectricLine electricLine;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Warrior>())
        {
            OnWarriorCross?.Invoke();
            ElectricLine line = Instantiate(electricLine, Vector2.zero, Quaternion.identity);
            line.Init(other.transform);
        }
        }


}
