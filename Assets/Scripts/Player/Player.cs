using UnityEngine;

public class Player : Entity
{

    [SerializeField] private CastManager castManager;
    
    private float currentAngle = 0f;

    void Update()
    {
        RotateTowardsMouse();
        castManager.TryToShootBasicArrow(transform.rotation);
        CheckIfSpellIsTriggered();
    }
    

    private void RotateTowardsMouse()
    {
        // Get the mouse position in world coordinates
        Vector3 mousePosition = MainCamera.Camera.MatchMouseCoordinatesToCamera(Input.mousePosition);

        // Calculate the direction from the sprite to the mouse position
        Vector3 direction = mousePosition - transform.position;


        // Calculate the angle between the sprite's current direction and the target direction
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Clamp the angle between -60 and 60 degrees
        float clampedAngle = Mathf.Clamp(targetAngle, -65f, 65f);

        // Smoothly interpolate towards the clamped angle to retain smooth movement
        currentAngle = Mathf.LerpAngle(currentAngle, clampedAngle, Time.deltaTime * 10f);

        // Apply the rotation to the sprite
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, currentAngle));
    }

    private void CheckIfSpellIsTriggered()
    {
        KeyCode key = InputManager.Instance.CheckKeyPressed();
        if (key != KeyCode.None)
        {
            print("?");
            castManager.TryToCastSpell(key);
        }

    }
    
}