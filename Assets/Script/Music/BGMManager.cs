using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    AudioSource audioSource;

    [Header("BGM Clips")]
    public AudioClip fieldBGM;
    public AudioClip battleBGM;
    public AudioClip endingBGM;   // ✅ 추가

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

    void Start()
    {
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Battle Scene")
        {
            PlayBattleBGM();
        }
        else if (scene.name == "Ending Scene")   // ✅ 엔딩씬 분기
        {
            PlayEndingBGM();
        }
        else
        {
            PlayFieldBGM();  // Start Scene 포함
        }
    }

    public void PlayFieldBGM()
    {
        PlayBGM(fieldBGM);
    }

    public void PlayBattleBGM()
    {
        PlayBGM(battleBGM);
    }

    public void PlayEndingBGM()   // ✅ 추가
    {
        PlayBGM(endingBGM);
    }

    void PlayBGM(AudioClip clip)
    {
        if (clip == null)
            return;

        // 이미 같은 곡 재생 중이면 중복 재시작 방지
        if (audioSource.isPlaying && audioSource.clip == clip)
            return;

        audioSource.clip = clip;
        audioSource.Play();
    }
}