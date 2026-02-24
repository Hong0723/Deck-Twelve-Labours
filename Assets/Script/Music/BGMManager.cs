using UnityEngine;
using UnityEngine.SceneManagement;

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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Battle Scene")
        {
            PlayBattleBGM();
        }
        else
        {
            PlayFieldBGM();
        }
    }
    void Start()
    {
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
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
     // 이미 같은 BGM이 재생 중이면 아무 것도 안 함
    if (audioSource.isPlaying && audioSource.clip == clip)
        return;

        audioSource.clip = clip;
        audioSource.Play();
    }
}
