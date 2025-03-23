using UnityEngine;

public class GameManager : MonoBehaviour
{
    static public GameManager Instance;

    [Range(0f,1f)] public float anger;



    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

}
