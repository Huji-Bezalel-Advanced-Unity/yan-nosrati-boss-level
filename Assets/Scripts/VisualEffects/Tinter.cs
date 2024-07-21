using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tinter : MonoBehaviour
{

    [SerializeField] private float startTintValue;
    [SerializeField] private Material bowMaterial;
    private float reductionRate = 0.08f;
    private float currentTintValue;


    private void Start()
    {
        currentTintValue = startTintValue;
        bowMaterial.SetFloat("_AuraSize", startTintValue);

        
    }

    // Update is called once per frame
    private void OnEnable()
    {
        ObjectCrossMapTrigger.OnWarriorCross += TintBow;
    }
    private void OnDisable()
    {
        ObjectCrossMapTrigger.OnWarriorCross -= TintBow;
    }

    private void TintBow()
    {
        currentTintValue -= reductionRate;
        bowMaterial.SetFloat("_AuraSize", currentTintValue);
    }
}
