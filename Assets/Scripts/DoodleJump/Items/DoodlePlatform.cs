using System.Collections;
using UnityEngine;

public class DoodlePlatform : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private bool isBreakable = false;
    [SerializeField] private bool isMoving = false;
    [SerializeField] private bool isOneWay = false;

    [Header("Breakable Settings")]
    [SerializeField] private float destroyDelay = 0.5f;
    [SerializeField] private Sprite brokenPlatform;

    [Header("Moving Settings")]
    private Vector2 pointA;
    [SerializeField] private Vector2 pointB;
    [SerializeField] private float speed = 2f;
    [SerializeField] private bool continuousMovement = false;

    private Vector2 target;
    private Vector2 direction;
    private bool isBreaking = false;

    private void Start()
    {
        if (isMoving)
        {
            target = pointB;
            direction = (target - (Vector2)transform.position).normalized;
        }
    }

    private void Update()
    {
        if (isMoving) MovePlatform();
    }

    private void MovePlatform()
    {
        transform.position += (Vector3)(speed * Time.deltaTime * direction);

        if (!continuousMovement)
        {
            if (Vector2.Distance(transform.position, target) < 0.1f)
            {
                target = target == pointA ? pointB : pointA;
                direction = (target - (Vector2)transform.position).normalized;
            }
        }
        if (transform.position.x > PlatformManager.screenWidth)
        {
            transform.position = new Vector2(-PlatformManager.screenWidth, transform.position.y);
            if (target == pointA) pointB.x += PlatformManager.screenWidth * 2;
            else pointA.x += PlatformManager.screenWidth * 2;
            target.x -= PlatformManager.screenWidth*2;
            
        }
        if (transform.position.x < -PlatformManager.screenWidth)
        {
            transform.position = new Vector2(PlatformManager.screenWidth, transform.position.y);
            if (target == pointA) pointB.x -= PlatformManager.screenWidth * 2;
            else pointA.x -= PlatformManager.screenWidth * 2;

            target.x += PlatformManager.screenWidth * 2;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (isBreakable && !isBreaking && collision.gameObject.TryGetComponent(out DoodleJumpPlayer player) && player.GetComponent<Rigidbody2D>().linearVelocityY < 0.001)
        {
            isBreaking = true;
            StartCoroutine(BreakPlatform());
            SoundManager.PlaySound(SoundType.PlatformBreak, 1.0f);
        }
    }

    private IEnumerator BreakPlatform()
    {
        
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = brokenPlatform;
        //Faire tomber la plateforme
        float y = 0;
        while (y < 6)
        {
            transform.position -= new Vector3(0, Time.deltaTime*2, 0);
            y+=Time.deltaTime*2;
            yield return null;
        }
        Destroy(gameObject);
    }

    public bool GetIsBreakable()
    {
        return isBreakable;
    }

    public void SetPlateforme(float speed, Vector2 pointA, Vector2 pointB)
    {
        this.speed = speed;
        this.pointB = pointB;
        this.pointB.y += transform.position.y;
        this.pointA = pointA;
        this.pointA.y += transform.position.y;

    }
}
