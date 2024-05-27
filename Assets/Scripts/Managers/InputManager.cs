using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class InputManager
{
    public static InputManager Instance;
    public static event Action<KeyCode> keyPressed;


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
            keyPressed?.Invoke(KeyCode.Q);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            keyPressed?.Invoke(KeyCode.W);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            keyPressed?.Invoke(KeyCode.R);
        }
    }
}

