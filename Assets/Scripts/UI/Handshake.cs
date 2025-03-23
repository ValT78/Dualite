using UnityEngine;

public class Handshake : MonoBehaviour
{
    [SerializeField] private float angleMin = -10f; // Angle minimum d'oscillation
    [SerializeField] private float angleMax = 10f;  // Angle maximum d'oscillation

    private float timeOffset;

    private void Start()
    {
        // Décale l'animation pour éviter une synchronisation parfaite avec d'autres objets
        timeOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    private void Update()
    {
        // Calcule l'angle basé sur un sinus oscillant entre -1 et 1
        float angle = Mathf.Lerp(angleMin, angleMax, (Mathf.Sin((Time.time + timeOffset) * 1 * Mathf.PI * 4) + 1f) / 2f);

        // Applique la rotation
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
