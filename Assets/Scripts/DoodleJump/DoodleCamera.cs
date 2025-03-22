using UnityEngine;

public class DoodleCamera : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float followOffset; // Distance au-dessus du joueur
    [SerializeField] private float autoScrollSpeed; // Vitesse de montée automatique
    [SerializeField] private float autoScrollRetardHeight;

    [Header("Components")]
    [SerializeField] private Transform player; // Référence au joueur


    private float autoScrollY; // Altitude minimale de la caméra
    private float playerMaxY;

    void Start()
    {
        autoScrollY = -2;
    }

    void Update()
    {
        autoScrollY += autoScrollSpeed * Time.deltaTime;
        if (player)
        {
            playerMaxY = Mathf.Max(playerMaxY, player.position.y);
            if(autoScrollY < playerMaxY - autoScrollRetardHeight) autoScrollY = playerMaxY - autoScrollRetardHeight;

            transform.position = new Vector3(transform.position.x, Mathf.Max(playerMaxY, autoScrollY) + followOffset, transform.position.z);
        }   
    }
}
