using UnityEngine;
using UnityEngine.InputSystem; // if you want to read InputValue
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 5f;          // Force for max height jump
    [SerializeField] private float jumpGravityScale = 1f;   // Normal gravity scale
    [SerializeField] private float fallGravityScale = 2.5f; // Higher gravity when falling/release jump

    private Rigidbody2D rb;
    private bool isGrounded = true;
    private AudioSource audioSource;
    private bool isJumping = false;

    private Vector3 startPosition;

    private bool jumpRequested = false;
    private bool jumpReleased = false;

    private HashSet<Collider2D> triggeredScoreZones = new HashSet<Collider2D>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        startPosition = transform.position;  // Store initial position
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            jumpRequested = true;
        }
        else if (!value.isPressed && isJumping)
        {
            jumpReleased = true;
        }
    }

    private void FixedUpdate()
    {
        // Handle jump request
        if (jumpRequested)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            rb.gravityScale = jumpGravityScale;
            isGrounded = false;
            isJumping = true;

            if (audioSource != null)
            {
                audioSource.PlayOneShot(audioSource.clip);
            }

            jumpRequested = false;
        }

        // Handle jump release
        if (jumpReleased)
        {
            if (rb.velocity.y > 0)
            {
                rb.gravityScale = fallGravityScale;
            }
            isJumping = false;
            jumpReleased = false;
        }

        // Apply higher gravity when falling
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = fallGravityScale;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // Notify the game manager
            GameManager.Instance.GameOver();
        }
        else if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
            rb.gravityScale = jumpGravityScale; // Reset to normal gravity
        }
    }

    // If using a separate "Restart" action
    public void OnRestart()
    {
        // we can broadcast an event, or talk to the GameManager directly.
        // We'll do it via an event in the GameManager for a cleaner approach.
        GameManager.Instance.RestartGame();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ScoreZone") && !triggeredScoreZones.Contains(collision))
        {
            GameManager.Instance.AddScore(1);
            triggeredScoreZones.Add(collision);
        }
    }

    public void OnToggle()
    {
        GameManager.Instance.ToggleMode();
    }

    public void ResetPosition()
    {
        transform.position = startPosition;
        rb.velocity = Vector2.zero;  // Reset velocity
        isGrounded = true;
        triggeredScoreZones.Clear();  // Clear the set when resetting
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGameRestart.AddListener(ResetPosition);
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameRestart.RemoveListener(ResetPosition);
    }
}
