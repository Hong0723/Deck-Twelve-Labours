using UnityEngine;

public class GamePauseManager : MonoBehaviour
{
    public static GamePauseManager Instance { get; private set; }

    private int pauseRequests = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RequestPause(string reason = "")
    {
        pauseRequests++;
        Apply();
        // Debug.Log($"[Pause] + {reason} count={pauseRequests}");
    }

    public void ReleasePause(string reason = "")
    {
        pauseRequests = Mathf.Max(0, pauseRequests - 1);
        Apply();
        // Debug.Log($"[Pause] - {reason} count={pauseRequests}");
    }

    private void Apply()
    {
        Time.timeScale = pauseRequests > 0 ? 0f : 1f;
    }
}