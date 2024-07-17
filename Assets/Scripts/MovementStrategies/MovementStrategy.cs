using UnityEngine;

namespace MovementStrategies
{
    public interface MovementStrategy
    {
        public void Move(Boss boss,Rigidbody2D rb, Transform transform, Vector2 direction, float moveSpeed);
    }
}