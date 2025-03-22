using UnityEngine;

public class DoodleCamera : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float followOffset = 3f; // Distance au-dessus du joueur
    [SerializeField] private float autoScrollSpeed = 1.5f; // Vitesse de montée automatique

    [Header("Components")]
    [SerializeField] private Transform player; // Référence au joueur


    private float lowestY; // Altitude minimale de la caméra

    void Start()
    {
        lowestY = transform.position.y;
    }

    void Update()
    {
        if (player != null)
        {
            float targetY = Mathf.Max(lowestY + autoScrollSpeed * Time.time, player.position.y + followOffset);
            transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
        }   
    }
}
