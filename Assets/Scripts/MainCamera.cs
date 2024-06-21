using System;
using System.Collections;
using System.Collections.Generic;
using Bosses;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class MainCamera : MonoBehaviour
{

    private Transform cameraTransform;
    private Vector3 originalPosition;
    private float currentShakeDuration = 0f;
    
    public static MainCamera Instance { get; private set; }
    private Camera _camera;

    private void OnEnable()
    {
        Player.OnPlayerChangePhase += TriggerShake;
        Player.OnTakingDamage += TriggerShake;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _camera = GetComponent<Camera>();
        cameraTransform = transform;
        originalPosition = cameraTransform.localPosition;
    }

    public Vector3 MatchMouseCoordinatesToCamera(Vector2 direction)
    {
        return _camera.ScreenToWorldPoint(direction);
    }
    public void TriggerShake(Phase phase)
    {
        StartCoroutine(Shake(0.5f,0.13f));
    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            Vector3 randomPoint = originalPosition + Random.insideUnitSphere * magnitude;

            cameraTransform.localPosition = randomPoint;

            elapsed += Time.deltaTime;

            yield return null;
        }

        cameraTransform.localPosition = originalPosition;
    }
}
