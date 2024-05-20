using UnityEngine;

public class InputManager : MonoBehaviour
{

    public static InputManager Instance;
  
    void Awake()
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

    public KeyCode CheckKeyPressed()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            return KeyCode.Q;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            return KeyCode.W;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            return KeyCode.R;
        }

        return KeyCode.None;
    }
}
