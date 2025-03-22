using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlatformSegment
{
    public string name;
    public float segmentHeight;
    public float minSpacing;
    public float maxSpacing;
    public float breakableChance;
    public float movingChance;
    public float monsterChance; // Probabilité qu'un monstre apparaisse sur une plateforme
}

public class PlatformManager : MonoBehaviour
{
    public static PlatformManager Instance;

    public static float screenWidth = 2.5f;

    [Header("Prefab")]
    [SerializeField] private GameObject basicPlatformPrefab;
    [SerializeField] private GameObject breakablePlatformPrefab;
    [SerializeField] private GameObject movingPlatformPrefab;
    [SerializeField] private GameObject coinPrefab;

    [Header("Monster Prefabs")]
    [SerializeField] private GameObject staticMonsterPrefab;
    [SerializeField] private GameObject jumpingMonsterPrefab;
    [SerializeField] private GameObject flyingMonsterPrefab;

    [Header("Segments Settings")]
    [SerializeField] private List<PlatformSegment> segments;
    private PlatformSegment currentSegment;

    [Header("Coin Settings")]
    [SerializeField] private float coinTrailChance;

    [Header("References")]
    [SerializeField] private CoinManager coinManager;
    [SerializeField] private Transform player;
    [SerializeField] private Transform platformContainer;
    [SerializeField] private Camera playerCamera;

    private float highestPlatformY;
    private float currentSegmentTop;
    private Queue<GameObject> activePlatforms = new Queue<GameObject>();

    private void Awake()
    {
        if (Instance) Destroy(gameObject);
        Instance = this;

        foreach(Transform child in platformContainer)
        {
            activePlatforms.Enqueue(child.gameObject);
        }
    }

    void Start()
    {
        currentSegment = segments[0]; // Premier tronçon fixe au départ
        currentSegmentTop = currentSegment.segmentHeight;
        highestPlatformY = GetHighestPlatformY();
    }

    void Update()
    {
        float camTop = playerCamera.transform.position.y + playerCamera.orthographicSize;

        if (highestPlatformY < camTop + currentSegment.maxSpacing)
        {
            print(camTop + currentSegment.maxSpacing);
            SpawnPlatform();
        }

        if (highestPlatformY >= currentSegmentTop)
        {
            ChooseNextSegment();
        }

        CleanupPlatforms();
    }

    private void SpawnPlatform()
    {
        float spawnY = highestPlatformY + Random.Range(currentSegment.minSpacing, currentSegment.maxSpacing);
        float spawnX = Random.Range(-screenWidth, screenWidth);

        GameObject newPlatform;
        float rand = Random.value;
        if (rand < currentSegment.breakableChance)
        {
            newPlatform = Instantiate(breakablePlatformPrefab, new Vector3(spawnX, spawnY, 0), Quaternion.identity, platformContainer);
        }
        else if (rand < currentSegment.breakableChance + currentSegment.movingChance)
        {
            newPlatform = Instantiate(movingPlatformPrefab, new Vector3(spawnX, spawnY, 0), Quaternion.identity, platformContainer);
        }
        else
        {
            newPlatform = Instantiate(basicPlatformPrefab, new Vector3(spawnX, spawnY, 0), Quaternion.identity, platformContainer);
        }

        activePlatforms.Enqueue(newPlatform);
        highestPlatformY = spawnY;

        // Générer une traînée de pièces aléatoirement
        if (Random.value < coinTrailChance)
        {
            coinManager.SpawnCoinTrail(new Vector2(spawnX, spawnY + 0.5f));
        }

        // Générer un monstre sur la plateforme
        if (Random.value < currentSegment.monsterChance)
        {
            SpawnMonster(newPlatform.transform);
        }
    }

    private void SpawnMonster(Transform platformTransform)
    {
        GameObject monsterPrefab;
        float rand = Random.value;

        if (rand < 0.33f)
        {
            monsterPrefab = staticMonsterPrefab;
        }
        else if (rand < 0.66f)
        {
            monsterPrefab = jumpingMonsterPrefab;
        }
        else
        {
            monsterPrefab = flyingMonsterPrefab;
        }

        Vector3 spawnPosition = platformTransform.position + new Vector3(0, 0.5f, 0);
        Instantiate(monsterPrefab, spawnPosition, Quaternion.identity, platformContainer);
    }

    private void ChooseNextSegment()
    {
        currentSegment = segments[Random.Range(1, segments.Count)]; // Choix aléatoire sauf le premier
        currentSegmentTop += currentSegment.segmentHeight;
    }

    private void CleanupPlatforms()
    {
        float camBottom = playerCamera.transform.position.y - playerCamera.orthographicSize;

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
