using System;
using System.Collections;
using UnityEngine;

public class WarriorCrossUpgrade : MonoBehaviour
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
        print("triggered");
        print(other.GetComponent<Warrior>());
        if (other.GetComponent<Warrior>())
        {
            // Reuse the ElectricLine instance
            print("called!");
            electricLineInstance.gameObject.SetActive(true); // Activate it
            OnWarriorCross?.Invoke();
            print("called1");
            electricLineInstance.Init(other.transform); // Initialize with new end position
            print("called2");

        }
    }
}