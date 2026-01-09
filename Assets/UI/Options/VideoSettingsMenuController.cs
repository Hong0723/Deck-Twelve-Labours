using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class VideoSettingsMenuController : MonoBehaviour
{
    private VisualElement root;
    private Button fullscreenButton;
    private Button resolutionButton;
    private Button backButton;

    private bool isFullscreen = false;

    private List<Vector2Int> resolutions = new List<Vector2Int>()
    {
        new Vector2Int(800, 640),
        new Vector2Int(1280, 720),
        new Vector2Int(1280, 800),
        new Vector2Int(1920, 1080)
    };

    private int currentResolutionIndex = 2; // 1280x800 기준

    void OnEnable()
    {
        var ui = GetComponent<UIDocument>();
        root = ui.rootVisualElement;
        root.visible = false;

        fullscreenButton = root.Q<Button>("fullscreen-toggle");
        resolutionButton = root.Q<Button>("resolution-button");
        backButton = root.Q<Button>("back-button");

        fullscreenButton.clicked += ToggleFullscreen;
        resolutionButton.clicked += ChangeResolution;

        backButton.clicked += () =>
        {
            root.visible = false;
            UIManager.Instance.ShowOptions();
        };

        RefreshUI();
    }

    private void ToggleFullscreen()
    {
        isFullscreen = !isFullscreen;
        Screen.fullScreen = isFullscreen;
        RefreshUI();
    }

    private void ChangeResolution()
    {
        currentResolutionIndex = (currentResolutionIndex + 1) % resolutions.Count;
        var res = resolutions[currentResolutionIndex];

        Screen.SetResolution(res.x, res.y, isFullscreen);
        RefreshUI();
    }

    private void RefreshUI()
    {
        fullscreenButton.text = isFullscreen ? "전체화면 (현재 상태)" : "창모드 (현재 상태)";

        var res = resolutions[currentResolutionIndex];
        resolutionButton.text = $"해상도: {res.x}x{res.y}";
    }

    public void Show()
    {
        root.visible = true;
    }

    void Update()
    {
        if (root.visible && Input.GetKeyDown(KeyCode.Escape))
        {
            root.visible = false;
            UIManager.Instance.ShowOptions();
        }
    }

}
