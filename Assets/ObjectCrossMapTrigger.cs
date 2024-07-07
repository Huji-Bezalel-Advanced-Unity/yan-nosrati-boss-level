using System;
using System.Collections;
using Spells;
using UnityEngine;

public class ObjectCrossMapTrigger : MonoBehaviour
{
    public static Action OnWarriorCross;
    [SerializeField] private ElectricLine electricLinePrefab;
    private ElectricLine electricLineInstance;

    private void Start()
    {
        // Instantiate the ElectricLine once
        electricLineInstance = Instantiate(electricLinePrefab, Vector3.zero, Quaternion.identity);
        electricLineInstance.gameObject.SetActive(false); // Initially inactive
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        Warrior warrior = other.GetComponent<Warrior>();

        if (warrior)
        {
            // Reuse the ElectricLine instance
            electricLineInstance.gameObject.SetActive(true); // Activate it
            OnWarriorCross?.Invoke();
            electricLineInstance.Init(other.transform);
            warrior.gameObject.SetActive(false);
            ObjectPoolManager.Instance.AddWarriorToPool(warrior);
        }

        Spell spell = other.GetComponent<Spell>();
        if (spell != null)
        {
            spell.gameObject.SetActive(false);
            ObjectPoolManager.Instance.AddSpellToPool(spell);

        }
    }
}