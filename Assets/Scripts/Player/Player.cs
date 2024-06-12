using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : Entity
{
    private CastManagerPlayer _castManager;
    private float currentAngle = 0f;
    [SerializeField] private GameObject bow;
    

    
    public void Init(CastManagerPlayer castManager, Transform healthBarUI)
    {
        _castManager = castManager;
        print(_castManager);
        healthBar = healthBarUI;
        health = 500;
        maxHealth = 500;
        
        
    }
    void Update()
    {
        InputManager.Instance.CheckKeyPressed();
        _castManager.UpdateSpellsCooldowns();
        Move();
        _castManager.TryToShootBasicArrow(bow.transform.rotation, transform.position);
        if (Input.GetKeyDown(KeyCode.A))
        {
            health -= 50;
        }
    }

    private void OnEnable()
    {
        InputManager.keyPressed += ReactToKeyPress;
    }


    protected override IEnumerator Die()
    {
        yield return null;
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
        Vector3 mousePos = MainCamera.Camera.MatchMouseCoordinatesToCamera(InputManager.Instance.GetMousePosition());
        _castManager.TryToCastSpell(key,
             new Vector3(Constants.BowPosition.x, mousePos.y,0),
            bow.transform.rotation);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        print("iht");
        SkeletonWarrior skeletonWarrior = col.gameObject.GetComponent<SkeletonWarrior>();
        if (skeletonWarrior)
        {
            print("??????");
            RemoveHealth(skeletonWarrior.damage);
        }
    }
    
    
   

    
    
}