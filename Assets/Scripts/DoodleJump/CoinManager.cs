using UnityEngine;
using System.Collections.Generic;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private Transform coinContainer;
    [SerializeField] private float coinSpacing = 0.5f;

    public void SpawnCoinTrail(Vector2 startPosition)
    {
        int patternType = Random.Range(0, 3); // 0: Vertical, 1: Horizontal, 2: Arc
        int coinCount = Random.Range(3, 7);
        float angle = Random.Range(-15f, 15f) * Mathf.Deg2Rad;

        switch (patternType)
        {
            case 0:
                SpawnVerticalTrail(startPosition, coinCount, angle);
                break;
            case 1:
                SpawnHorizontalTrail(startPosition, coinCount);
                break;
            case 2:
                SpawnArcTrail(startPosition, coinCount);
                break;
        }
    }

    private void SpawnVerticalTrail(Vector2 start, int count, float angle)
    {
        for (int i = 0; i < count; i++)
        {
            float offsetX = i * coinSpacing * Mathf.Sin(angle);
            float offsetY = i * coinSpacing * Mathf.Cos(angle);
            SpawnCoin(new Vector2(start.x + offsetX, start.y + offsetY));
        }
    }

    private void SpawnHorizontalTrail(Vector2 start, int count)
    {
        for (int i = 0; i < count; i++)
        {
            float offsetX = i * coinSpacing;
            float wrappedX = WrapPosition(start.x + offsetX);
            SpawnCoin(new Vector2(wrappedX, start.y));
        }
    }

    private void SpawnArcTrail(Vector2 start, int count)
    {
        float radius = 1.5f;
        float angleStep = Mathf.PI / (count - 1);

        for (int i = 0; i < count; i++)
        {
            float angle = -Mathf.PI / 2 + i * angleStep;
            float offsetX = radius * Mathf.Cos(angle);
            float offsetY = radius * Mathf.Sin(angle);
            float wrappedX = WrapPosition(start.x + offsetX);
            SpawnCoin(new Vector2(wrappedX, start.y + offsetY));
        }
    }

    private void SpawnCoin(Vector2 position)
    {
        Instantiate(coinPrefab, position, Quaternion.identity, coinContainer);
    }

    private float WrapPosition(float x)
    {
        if (x > PlatformManager.screenWidth) return x - 2 * PlatformManager.screenWidth;
        if (x < -PlatformManager.screenWidth) return x + 2 * PlatformManager.screenWidth;
        return x;
    }
}
