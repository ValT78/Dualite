using UnityEngine;

public enum SoundType
{
    Jump,
    Death,
    Shoot1,
    Shoot2,
    Shoot3,
    Mob,
    Kill,
    Hit,
    Coin,
    SuperCoin,
    Spring,
    Pause,
    Resume,
    Quit,
    Button,
    PlatformBreak,
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    private static SoundManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        instance.audioSource.PlayOneShot(instance.soundList[(int)sound], volume);
    }
}
