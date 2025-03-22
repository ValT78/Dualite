using UnityEngine;

public class GameManager : MonoBehaviour
{
    static public GameManager Instance;

    [Range(0f,1f)] public float anger;



    private void Awake()
    {
        if (GameManager.Instance == null) Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
