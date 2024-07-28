using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using Managers.Pool;
using UnityEngine;
using UnityEngine.UI;

public class WarriorSpellUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private PoolName poolname;
    private Coroutine updateUiCoroutine;
    void Awake()
    {
        CoreManager.Instance.EventManager.AddListener(EventNames.OnSpellCast, UpdateUI);
    }

    private void OnDestroy()
    {
        CoreManager.Instance.EventManager.RemoveListener(EventNames.OnSpellCast, UpdateUI);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            CoreManager.Instance.EventManager.InvokeEvent(EventNames.OnSpellCast, (5f,PoolName.VillageWarriorPrefab));
        }
    }

    private void UpdateUI(object obj)
    {
        if (obj is (float duration, PoolName pn))
        {
            if (poolname != pn)
            {
                return;
            }
            if (updateUiCoroutine != null)
            {
                StopCoroutine(updateUiCoroutine);
            }
            updateUiCoroutine = StartCoroutine(Util.DoFillLerp(fillImage, 1, 0, duration));
        }
    }
}
