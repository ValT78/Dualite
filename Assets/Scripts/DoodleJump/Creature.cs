using System.Collections;
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

    [Header("Vibration Settings")]
    public float vibrationIntensity = 0.1f;
    public float vibrationSpeed = 3f;

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

        screenWidth = PlatformManager.screenWidth + 1f; // +1 pour marge de sécurité

        StartCoroutine(ApplyVibration());

        SoundManager.PlaySound(SoundType.Mob, 0.7f);
    }

    void Update()
    {
        if (behavior == BehaviorType.Flying)
        {
            Fly();
        }
    }

    IEnumerator ApplyVibration()
    {
        while (true)
        {
            float targetX = initialPosition.x + (Mathf.PerlinNoise(Time.time * vibrationSpeed, 0f) - 0.5f) * vibrationIntensity;
            float targetY = initialPosition.y + (Mathf.PerlinNoise(0f, Time.time * vibrationSpeed) - 0.5f) * vibrationIntensity;

            Vector3 targetPosition = new Vector3(targetX, targetY, initialPosition.z);
            float duration = 0.1f; // Durée de transition fluide
            float elapsed = 0f;

            Vector3 startPosition = transform.position;

            while (elapsed < duration)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPosition;

            yield return new WaitForSeconds(0.05f); // Pause entre les vibrations
        }
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
        SoundManager.PlaySound(SoundType.Kill, 0.6f);
    }
}
