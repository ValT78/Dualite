using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class DoodleJumpPlayer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float deathHeightOffset;
    [SerializeField] private float shootCooldown = 0.5f; // Temps entre chaque tir

    [Header("Shooting")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint; // Position où le projectile apparaît

    [Header("Animation")]
    [SerializeField] private float deathJumpForce;
    [SerializeField] private float deathSideForce;
    [SerializeField] private float deathTorque;
    [SerializeField] private float timeBeforeDestroy;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D col;
    [SerializeField] private Camera playerCamera;

    [Header("Sprite")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites; // [0] = normal, [1] = jump, [2] = attack

    private Vector2 moveInput;
    private bool isDead;
    private bool canShoot = true;

    void Update()
    {
        if (isDead) return;


        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

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
        if (moveInput.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (moveInput.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    public void OnAttack(InputValue value)
    {
        if (canShoot && !isDead)
        {
            StartCoroutine(Shoot());
            float randomShootSound = Random.Range(0.5f, 2.0f);
            if (randomShootSound > 1.5)
            {
                SoundManager.PlaySound(SoundType.Shoot1, 1);
            } else if (randomShootSound < 1.0)
            {
                SoundManager.PlaySound(SoundType.Shoot2, 1);
            } else
            {
                SoundManager.PlaySound(SoundType.Shoot3, 1);
            }
        }
    }

    private IEnumerator Shoot()
    {
        canShoot = false;

        // Instancier le projectile au-dessus du joueur
        Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        StartCoroutine(AttackSprite());

        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out DoodlePlatform _))
        {
            if (rb.linearVelocity.y <= 0.0001)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                SoundManager.PlaySound(SoundType.Jump, 0.3f);
                StartCoroutine(JumpSprite());
            }
        }
        
    }

    private IEnumerator JumpSprite()
    {
        spriteRenderer.sprite = sprites[1];
        yield return new WaitForSeconds(0.7f);
        spriteRenderer.sprite = sprites[0];
    }

    private IEnumerator AttackSprite()
    {
        spriteRenderer.sprite = sprites[2];
        yield return new WaitForSeconds(0.4f);
        spriteRenderer.sprite = sprites[0];
    }


    public void Die()
    {
        isDead = true;
        rb.linearVelocity = new Vector2(Random.Range(-deathSideForce, deathSideForce), deathJumpForce);
        rb.constraints = RigidbodyConstraints2D.None;
        rb.angularVelocity = deathTorque;
        col.enabled = false;
        SoundManager.PlaySound(SoundType.Death, 1);
        StartCoroutine(ComeBackToMenu());
    }

    private IEnumerator ComeBackToMenu()
    {
        yield return new WaitForSeconds(timeBeforeDestroy);
        GameManager.Instance.ResetScene();
    }
}
