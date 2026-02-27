using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    [Header("Fade UI (assign in Inspector)")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;

    [Header("Durations")]
    [SerializeField] private float fadeOutDuration = 0.25f;
    [SerializeField] private float fadeInDuration = 0.25f;

    private bool isTransitioning;

    [SerializeField] private Canvas fadeCanvas; // 추가: FadeCanvas의 Canvas

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (fadeCanvas != null)
            fadeCanvas.enabled = true; // 플레이 들어오면 켜기

        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.blocksRaycasts = false;
            fadeCanvasGroup.interactable = false;
        }
    }

    public void TransitionTo(string sceneName)
    {
        if (isTransitioning) return;
        StartCoroutine(TransitionRoutine(sceneName));
    }

    private IEnumerator TransitionRoutine(string sceneName)
    {
        isTransitioning = true;

        // 1) Fade Out
        yield return Fade(1f, fadeOutDuration);

        // 2) Async Load (씬 활성화는 잠시 대기)
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        // 로딩 진행 (보통 0.9까지)
        while (op.progress < 0.9f)
            yield return null;

        // 3) Activate
        op.allowSceneActivation = true;
        while (!op.isDone)
            yield return null;

        // 4) Fade In
        yield return Fade(0f, fadeInDuration);

        isTransitioning = false;
    }

    private IEnumerator Fade(float targetAlpha, float duration)
    {
        if (fadeCanvasGroup == null) yield break;

        fadeCanvasGroup.blocksRaycasts = true;

        float startAlpha = fadeCanvasGroup.alpha;
        float t = 0f;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float p = duration <= 0f ? 1f : (t / duration);
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, p);
            yield return null;
        }

        fadeCanvasGroup.alpha = targetAlpha;

        if (Mathf.Approximately(targetAlpha, 0f))
            fadeCanvasGroup.blocksRaycasts = false;
    }
}