using UnityEngine;

namespace DefaultNamespace.MovementStrategies
{
    public class CircularMovementStrategy : MovementStrategy
    {
        private float angle = 0f;
        float circulatMovementPenatly = 5;


        public void Move(Boss boss, Rigidbody2D rb, Transform transform, Vector2 direction, float moveSpeed)
        {
            angle += moveSpeed/circulatMovementPenatly* Time.deltaTime*direction.y;

            // Clamp the angle to stay within the half-circle (0 to π)
            if (angle > Mathf.PI/2)
            {
                angle = Mathf.PI/2;
                boss.direction = Vector2.down;
            }

            else if (angle < -Mathf.PI / 2)
            {
                angle = -Mathf.PI / 2;
                boss.direction = Vector2.up;
            }
        
            // Calculate the new position on the half-circle
            float x = -7 * Mathf.Cos(angle);
            float y = 7* Mathf.Sin(angle);

            // Set the new position relative to the pivot point
            rb.MovePosition(new Vector3(Constants.StartingBossXPossition,0,0) + new Vector3(x, y, 0));
        }
    }
}