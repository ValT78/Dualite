using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    [SerializeField] private GameObject scorePrefab;
    [SerializeField] private Sprite superSprite;
    [SerializeField] private float superProbability;
    [SerializeField] private float score;
    [SerializeField] private float superScore;
    [SerializeField] private float bounceScale = 1.3f;
    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private float rotationDuration = 0.2f;

    private bool isSuperCoin;
    private static int coinCount = 0;
    private static float totalScore = 0;

    private void Start()
    {
        if (Random.value < superProbability)
        {
            isSuperCoin = true;
            GetComponent<SpriteRenderer>().sprite = superSprite;
            score = superScore;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out DoodleJumpPlayer _))
        {
            totalScore += score;
            coinCount++;

            // Afficher le score toutes les 5 pièces collectées
            if (coinCount % 5 == 0)
            {
                Instantiate(scorePrefab).GetComponent<ScoreText>().SetUp(transform.position, coinCount.ToString());
            }

            // Appliquer les effets visuels avant destruction
            StartCoroutine(CollectEffect());

           
        }
    }

    private IEnumerator CollectEffect()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        // 1. Scale Bounce
        transform.localScale *= bounceScale;
        yield return new WaitForSeconds(0.05f);
        transform.localScale /= bounceScale;

        // 2. Flash Blanc
        sr.color = Color.white;
        yield return new WaitForSeconds(flashDuration);
        sr.color = Color.yellow; // Remettre à sa couleur d'origine

        // 3. Rotation 3D (simulé avec scale X)
        float elapsedTime = 0;
        while (elapsedTime < rotationDuration)
        {
            float t = elapsedTime / rotationDuration;
            float scaleX = Mathf.Lerp(1, 0, t);
            transform.localScale = new Vector3(scaleX, 1, 1);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Destruction après les effets
        Destroy(gameObject);
    }
}
