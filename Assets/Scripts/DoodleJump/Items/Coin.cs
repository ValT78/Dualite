using UnityEngine;
using System.Collections.Generic;


public class Coin : MonoBehaviour
{
    [SerializeField] private Sprite superSprite;
    [SerializeField] private float superProbability;
    [SerializeField] private float score;
    [SerializeField] private float superScore;

    private bool isSuperCoin;

    private void Start()
    {
        // 20% de chance d'être une super pièce
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
            // Ajouter des points au joueur (à implémenter selon ton système de score)
            Destroy(gameObject);
        }
    }
}
