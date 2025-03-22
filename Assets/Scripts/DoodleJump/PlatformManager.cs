using UnityEngine;
using System.Collections.Generic;

public class PlatformManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private float platformSpacing = 2.5f;
    [SerializeField] private float minX = -4f, maxX = 4f;
    [SerializeField] private int maxPlatforms = 10;

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform platformContainer;

    private float highestPlatformY;
    private Queue<GameObject> activePlatforms = new Queue<GameObject>();

    void Start()
    {
        highestPlatformY = GetHighestPlatformY();
    }

    void Update()
    {
        float camTop = Camera.main.transform.position.y + Camera.main.orthographicSize;

        // Générer de nouvelles plateformes
        while (highestPlatformY < camTop + platformSpacing)
        {
            SpawnPlatform();
        }

        // Supprimer les plateformes hors écran
        CleanupPlatforms();
    }

    private void SpawnPlatform()
    {
        float spawnY = highestPlatformY + platformSpacing;
        float spawnX = Random.Range(minX, maxX);

        GameObject newPlatform = Instantiate(platformPrefab, new Vector3(spawnX, spawnY, 0), Quaternion.identity, platformContainer);
        activePlatforms.Enqueue(newPlatform);
        highestPlatformY = spawnY;
    }

    private void CleanupPlatforms()
    {
        float camBottom = Camera.main.transform.position.y - Camera.main.orthographicSize;

        while (activePlatforms.Count > 0 && activePlatforms.Peek().transform.position.y < camBottom)
        {
            Destroy(activePlatforms.Dequeue());
        }
    }

    private float GetHighestPlatformY()
    {
        float highestY = float.MinValue;
        foreach (Transform platform in platformContainer)
        {
            if (platform.position.y > highestY)
                highestY = platform.position.y;
        }
        return highestY;
    }
}
