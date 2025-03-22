using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    [SerializeField] private PlatformEffector2D effector;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out DoodleJumpPlayer player))
        {
            // Si le joueur arrive du bas, on lui permet de traverser
            if (player.transform.position.y < transform.position.y)
            {
                effector.rotationalOffset = 180f;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out DoodleJumpPlayer _))
        {
            // Dès que le joueur passe au-dessus, il ne peut plus traverser
            effector.rotationalOffset = 0f;
        }
    }
}
