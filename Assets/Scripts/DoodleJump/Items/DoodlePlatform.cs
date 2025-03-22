using UnityEngine;

public class DoodlePlatform : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private bool isBreakable = false;
    [SerializeField] private bool isMoving = false;
    [SerializeField] private bool isOneWay = false;

    [Header("Breakable Settings")]
    [SerializeField] private float destroyDelay = 0.5f;

    [Header("Moving Settings")]
    [SerializeField] private Vector2 pointA;
    [SerializeField] private Vector2 pointB;
    [SerializeField] private float speed = 2f;
    [SerializeField] private bool continuousMovement = false;

    [Header("References")]
    private Collider2D platformCollider;

    private Vector2 target;
    private Vector2 direction;
    private bool isBreaking = false;

    private void Awake()
    {
        platformCollider = GetComponent<Collider2D>();
        pointA.y = transform.position.y;
        pointB.y += transform.position.y;
    }

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
            Destroy(gameObject);
        }
    }

    public bool GetIsBreakable()
    {
        return isBreakable;
    }
}
