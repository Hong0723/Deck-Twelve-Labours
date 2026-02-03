using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    AudioSource audioSource;

    [Header("BGM Clips")]
    public AudioClip fieldBGM;
    public AudioClip battleBGM;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
    }

    void Start()
    {
        PlayFieldBGM();
    }

    public void PlayFieldBGM()
    {
        PlayBGM(fieldBGM);
    }

    public void PlayBattleBGM()
    {
        PlayBGM(battleBGM);
    }

    void PlayBGM(AudioClip clip)
    {
        if (audioSource.clip == clip) return;

        audioSource.clip = clip;
        audioSource.Play();
    }
}
