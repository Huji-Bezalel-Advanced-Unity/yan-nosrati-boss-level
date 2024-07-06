using System;
using UnityEngine;

public class InputManager
{
    public static InputManager Instance;
    public static event Action<KeyCode> KeyPressed;


    public InputManager()
    {
        if (Instance == null)
        {
            Instance = this;
        }

    }

    public Vector3 GetMousePosition()
    {
        return Input.mousePosition;
    }

    
    public void CheckKeyPressed()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            KeyPressed?.Invoke(KeyCode.Q);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            KeyPressed?.Invoke(KeyCode.W);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            KeyPressed?.Invoke(KeyCode.R);
        }
    }
}

