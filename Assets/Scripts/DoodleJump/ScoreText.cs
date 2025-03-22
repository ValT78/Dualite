using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    [SerializeField] private float upTime;
    [SerializeField] private float speed;

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private TextMeshProUGUI textMesh;
    private void Awake()
    {
        print(rectTransform.gameObject.name);
        StartCoroutine(UpScoreText());
    }

    public void SetUp(Vector3 position, string score)
    {
        rectTransform.anchoredPosition = position;
        textMesh.text = score;
    }

    //Coroutine qui fait monté le texte jusqu'à disparaitre
    private IEnumerator UpScoreText()
    {
        float t = 0;
        while (t < upTime)
        {
            yield return null;
            t += Time.deltaTime;
            rectTransform.anchoredPosition += new Vector2(0, speed * Time.deltaTime);
        }
        Destroy(gameObject);
         
    }
}
