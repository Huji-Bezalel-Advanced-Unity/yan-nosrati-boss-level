using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Color _flashColor = Color.white;
    [SerializeField] private float flashTime = 0.25f;

    private SpriteRenderer[] _spriteRenderers;
    private Material[] _materials;
    private Coroutine flashCoroutine;

    private void Awake()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        Init();
    }

    private void Init()
    {
        _materials = new Material[_spriteRenderers.Length];
        for (int i = 0; i < _spriteRenderers.Length; ++i)
        {
            _materials[i] = _spriteRenderers[i].material;
        }
    }

    public void CallFlasher()
    {
         flashCoroutine = StartCoroutine(Flasher());
    }
    private IEnumerator Flasher()
    {
        SetFlashColor();
        
        float elapsedTime = 0;
        float currentColorAmount = 0f;
        while (elapsedTime < flashTime)
        {
            elapsedTime += Time.deltaTime;
            float precentageCompleted = elapsedTime / flashTime;
            currentColorAmount = Mathf.Lerp(1, 0, precentageCompleted);
            SetFlashAmount(currentColorAmount);
            yield return null;
        }
    }
    

    private void SetFlashColor()
    {
        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i].SetColor("_FlashColor",_flashColor);
        }
    }

    private void SetFlashAmount(float amount)
    {
        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i].SetFloat("_FlashAmount", amount);
        }
    }

}
