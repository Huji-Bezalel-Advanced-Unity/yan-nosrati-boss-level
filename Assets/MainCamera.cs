using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public static MainCamera Camera { get; private set; }
    [SerializeField] private Camera _camera;

    private void Awake()
    {
        if (Camera == null)
        {
            Camera = this;
        }
    }

    public Vector3 MatchMouseCoordinatesToCamera(Vector2 direction)
    {
        return _camera.ScreenToWorldPoint(direction);
    }
}
