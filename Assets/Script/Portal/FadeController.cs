using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    public static FadeController Instance { get; private set; }

    [SerializeField] private Image fadeImage;
    private Coroutine _co;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (fadeImage != null)
        {
            var c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
            fadeImage.raycastTarget = true; // 페이드 중 클릭/입력 차단용
        }
    }

    public void StopFade()
    {
        if (_co != null) StopCoroutine(_co);
        _co = null;
    }

    public IEnumerator FadeTo(float targetAlpha, float duration)
    {
        if (fadeImage == null) yield break;

        float startAlpha = fadeImage.color.a;
        float t = 0f;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float a = Mathf.Lerp(startAlpha, targetAlpha, t / duration);

            var c = fadeImage.color;
            c.a = a;
            fadeImage.color = c;

            yield return null;
        }

        var end = fadeImage.color;
        end.a = targetAlpha;
        fadeImage.color = end;
    }
}
