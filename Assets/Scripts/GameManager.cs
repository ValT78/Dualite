using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static public GameManager Instance;

    [Range(0f,1f)] public float anger;
    [SerializeField] Material speed;
    [SerializeField] Material blood;



    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Update()
    {
        if (anger > 1f) ResetScene();
    }

    public void ResetScene()
    {
        speed.SetFloat("_Alpha", 0f);
        blood.SetFloat("_Alpha", 0f);
        SceneManager.LoadScene("menu");
    }
}
