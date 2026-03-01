using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BattleBackgroundFitter : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private Sprite defaultBackground;

    [Tooltip("true면 화면을 가득 채움(여백 방지).")]
    [SerializeField] private bool coverScreen = true;

    [Tooltip("전환/카메라 이동/해상도 변경에도 계속 맞춤")]
    [SerializeField] private bool keepFitting = true;

    private SpriteRenderer sr;
    private int lastW, lastH;
    private float lastAspect;
    private Vector3 lastCamPos;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (targetCamera == null) targetCamera = Camera.main;
    }

    private void Start()
    {
        var ctx = CombatContext.Instance;
        sr.sprite = (ctx != null && ctx.selectedBattleBackground != null)
            ? ctx.selectedBattleBackground
            : defaultBackground;

        ApplyFit(true);
    }

    private void LateUpdate()
    {
        if (!keepFitting) return;
        ApplyFit(false);
    }

    private void ApplyFit(bool force)
    {
        if (sr.sprite == null || targetCamera == null) return;

        bool changed =
            force ||
            Screen.width != lastW ||
            Screen.height != lastH ||
            Mathf.Abs(targetCamera.aspect - lastAspect) > 0.0001f ||
            targetCamera.transform.position != lastCamPos;

        if (!changed) return;

        lastW = Screen.width;
        lastH = Screen.height;
        lastAspect = targetCamera.aspect;
        lastCamPos = targetCamera.transform.position;

        Fit(sr.sprite, targetCamera, coverScreen);
    }

    private void Fit(Sprite sprite, Camera cam, bool cover)
    {
        float viewW, viewH;

        if (cam.orthographic)
        {
            viewH = cam.orthographicSize * 2f;
            viewW = viewH * cam.aspect;
        }
        else
        {
            float zDist = Mathf.Abs(transform.position.z - cam.transform.position.z);
            Vector3 bl = cam.ViewportToWorldPoint(new Vector3(0f, 0f, zDist));
            Vector3 tr = cam.ViewportToWorldPoint(new Vector3(1f, 1f, zDist));
            viewW = tr.x - bl.x;
            viewH = tr.y - bl.y;
        }

        Vector2 s = sprite.bounds.size;
        float sx = viewW / s.x;
        float sy = viewH / s.y;
        float scale = cover ? Mathf.Max(sx, sy) : Mathf.Min(sx, sy);

        transform.localScale = new Vector3(scale, scale, 1f);
        transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, transform.position.z);
    }
}