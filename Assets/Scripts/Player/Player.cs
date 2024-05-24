using Unity.VisualScripting;
using UnityEngine;

public class Player : Entity
{

    private CastManager _castManager;
    private float currentAngle = 0f;
    [SerializeField] private GameObject bow;
    

    
    public void Init(CastManager castManager)
    {
        _castManager = castManager;
        health = 500;
        maxHealth = 500;
        
    }
    void Update()
    {
        InputManager.Instance.CheckKeyPressed();
        _castManager.UpdateSpellsCooldowns();
        Move();
        _castManager.TryToShootBasicArrow(bow.transform.rotation);
        if (Input.GetKeyDown(KeyCode.A))
        {
            health -= 50;
        }
    }

    private void OnEnable()
    {
        InputManager.keyPressed += ReactToKeyPress;
    }
    

    public override void Move()
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
        bow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, currentAngle));
    }
    
    
    private void ReactToKeyPress(KeyCode key)
    {
        _castManager.TryToCastSpell(key,bow.transform.rotation);
    }
   

    
    
}