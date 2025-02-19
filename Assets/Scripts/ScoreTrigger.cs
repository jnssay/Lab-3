using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If obstacle hits this trigger, increment score
        if(collision.gameObject.CompareTag("Obstacle"))
        {
            GameManager.Instance.AddScore(1);
        }
    }
}
