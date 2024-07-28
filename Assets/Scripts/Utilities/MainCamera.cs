using System;
using System.Collections;
using System.Collections.Generic;
using Bosses;
using Managers;
using player;
using Unity.VisualScripting;
using UnityEngine;
using Warriors;
using Random = UnityEngine.Random;

public class MainCamera : MonoBehaviour
{
    private Transform _cameraTransform;
    private Vector3 _originalPosition;

    public static MainCamera Instance { get; private set; }
    public Camera _camera;

    private void OnEnable()
    {
        EventManager.Instance.AddListener(EventNames.OnPlayerChangePhase, TriggerShake);
        EventManager.Instance.AddListener(EventNames.OnPlayerTakingDamage, TriggerShake);
      
    }
    
    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(EventNames.OnPlayerChangePhase, TriggerShake);
        EventManager.Instance.RemoveListener(EventNames.OnPlayerTakingDamage, TriggerShake);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        _camera = GetComponent<Camera>();
        _cameraTransform = transform;
        _originalPosition = _cameraTransform.localPosition;
    }

    public Vector3 MatchMouseCoordinatesToCamera(Vector2 direction)
    {
        return _camera.ScreenToWorldPoint(direction);
    }

    public void TriggerShake(object obj)
    {
        StartCoroutine(Shake(0.25f, 0.1f));
    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            Vector3 randomPoint = _originalPosition + Random.insideUnitSphere * magnitude;
            _cameraTransform.localPosition = randomPoint;
            elapsed += Time.deltaTime;
            yield return null;
        }

        _cameraTransform.localPosition = _originalPosition;
    }
}