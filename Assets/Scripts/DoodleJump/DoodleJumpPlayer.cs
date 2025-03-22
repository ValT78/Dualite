using UnityEngine;
using UnityEngine.InputSystem;

public class DoodleJumpPlayer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float deathHeightOffset; // Distance sous la caméra avant de mourir

    [Header("Animation")]
    [SerializeField] private float deathJumpForce; // Saut au moment de la mort
    [SerializeField] private float deathSideForce;
    [SerializeField] private float deathTorque; // Rotation au moment de la mort
    [SerializeField] private float timeBeforeDestroy;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D col;
    [SerializeField] private Camera playerCamera;

    private Vector2 moveInput;
    private bool isDead;

    void Update()
    {
        if(isDead) return;

        rb.linearVelocityX = moveInput.x * moveSpeed;

        // Effet wrap (réapparaît de l'autre côté de l'écran)
        if (transform.position.x < -PlatformManager.screenWidth) transform.position = new Vector2(PlatformManager.screenWidth, transform.position.y);
        if (transform.position.x > PlatformManager.screenWidth) transform.position = new Vector2(-PlatformManager.screenWidth, transform.position.y);

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
        if (collision.gameObject.TryGetComponent(out DoodlePlatform platform) && !platform.GetIsBreakable())
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
        rb.constraints = RigidbodyConstraints2D.None;
        rb.angularVelocity = deathTorque;
        col.enabled = false;
        Destroy(gameObject, timeBeforeDestroy);
    }
}
