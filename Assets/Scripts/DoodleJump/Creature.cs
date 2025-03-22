using UnityEngine;

public class Creature : MonoBehaviour
{
    public enum BehaviorType { Static, Bouncing, Flying }
    public BehaviorType behavior;

    [Header("Bouncing Settings")]
    public float jumpForce = 5f;
    public float jumpInterval = 2f;

    [Header("Flying Settings")]
    public float speed = 2f;
    private float screenWidth;

    private Vector3 initialPosition;
    private Rigidbody2D rb;

    void Start()
    {
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();

        if (behavior == BehaviorType.Bouncing)
        {
            InvokeRepeating(nameof(Jump), 1f, jumpInterval);
        }

        screenWidth = Camera.main.orthographicSize * Camera.main.aspect + 1f; // +1 pour marge de sécurité
    }

    void Update()
    {
        ApplyVibration();

        if (behavior == BehaviorType.Flying)
        {
            Fly();
        }
    }

    void ApplyVibration()
    {
        float vibrationX = Mathf.PerlinNoise(Time.time * 3f, 0f) * 0.1f - 0.05f;
        float vibrationY = Mathf.PerlinNoise(0f, Time.time * 3f) * 0.1f - 0.05f;
        transform.position = initialPosition + new Vector3(vibrationX, vibrationY, 0);
    }

    void Jump()
    {
        if (rb)
        {
            rb.linearVelocityY = jumpForce;
        }
    }

    void Fly()
    {
        transform.position += speed * Time.deltaTime * Vector3.left;

        if (transform.position.x < -screenWidth)
        {
            transform.position = new Vector3(screenWidth, transform.position.y, transform.position.z);
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }
}
