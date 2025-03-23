using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static public GameManager Instance;

    [Range(0f,1f)] public float anger;



    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Update()
    {
        if (anger > 1f) SceneManager.LoadScene("menu");
    }
}
