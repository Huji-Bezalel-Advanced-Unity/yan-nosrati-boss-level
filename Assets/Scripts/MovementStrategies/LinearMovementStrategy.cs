using UnityEngine;

namespace MovementStrategies
{
    public class LinearMovementStrategy : MovementStrategy
    {
        public void Move(Boss boss,Rigidbody2D rb, Transform transform, Vector2 direction, float moveSpeed)
        {
            rb.MovePosition((Vector2)transform.position + direction * (Mathf.Sin(Time.deltaTime*moveSpeed) * 1.4f));

        }
    }
    
        
    
}