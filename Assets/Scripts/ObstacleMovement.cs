using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    void FixedUpdate()
    {
        transform.Translate(Vector2.left * speed * Time.fixedDeltaTime);

        // If it goes off screen to the left too far, destroy it
        if (transform.position.x < -20f)
        {
            Destroy(gameObject);
        }
    }
}
