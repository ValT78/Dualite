using UnityEngine;
using UnityEngine.InputSystem;

public class DoodleJumpPlayer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float deathHeightOffset = -5f; // Distance sous la caméra avant de mourir

    [Header("Animation")]
    [SerializeField] private float deathJumpForce = 15f; // Saut au moment de la mort
    [SerializeField] private float deathSideForce = 10f;
    [SerializeField] private float deathTorque = 200f; // Rotation au moment de la mort
    [SerializeField] private float timeBeforeDestroy = 2f;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D col;
    [SerializeField] private Camera playerCamera;

    private Vector2 moveInput;
    private bool isDead = false;

    void Update()
    {
        if(isDead) return;

        rb.linearVelocityX = moveInput.x * moveSpeed;

        // Effet wrap (réapparaît de l'autre côté de l'écran)
        if (transform.position.x < -5f) transform.position = new Vector2(5f, transform.position.y);
        if (transform.position.x > 5f) transform.position = new Vector2(-5f, transform.position.y);

        // Vérifier si le joueur tombe trop bas
        if (playerCamera.transform.position.y + deathHeightOffset > transform.position.y)
        {
            Die();
        }
    }

    public void OnDoodleMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out OneWayPlatform _))
        {
            // Saut uniquement si le joueur tombe (léger offset car la velocité est parfois positive
            if (rb.linearVelocityY <= 0.0001)
            {
                rb.linearVelocityY = jumpForce;
            }
        }
    }

    void Die()
    {
        isDead = true;
        rb.linearVelocity = new Vector2(Random.Range(-deathSideForce, deathSideForce), deathJumpForce);
        rb.angularVelocity = deathTorque;
        col.enabled = false;
        Destroy(gameObject, timeBeforeDestroy);
    }
}
