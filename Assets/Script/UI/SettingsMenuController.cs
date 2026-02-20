using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class SettingsMenuController : MonoBehaviour
{
    [Header("UI Toolkit")]
    [SerializeField] private UIDocument uiDocument;

    [Header("Keybind")]
    [SerializeField] private KeyCode toggleKey = KeyCode.Escape;

    private VisualElement root;
    private VisualElement overlay;

    private Button resumeButton;

    // 텍스트 토글 버튼으로 교체
    private Button fullscreenTextToggle;
    private Button masterVolumeTextToggle;

    private Slider volumeSlider;
    private Label volumeValueLabel;
    private Button quitButton;

    private bool isOpen;

    // 상태값
    private bool isFullscreen;
    private bool masterVolumeOn;

    private const string PREF_VOLUME = "settings_volume";         // 0~1
    private const string PREF_MASTER_ON = "settings_master_on";   // 0/1
    private const string PREF_FULLSCREEN = "settings_fullscreen"; // 0/1

    void Awake()
    {
        if (uiDocument == null)
            uiDocument = GetComponent<UIDocument>();

        root = uiDocument.rootVisualElement;

        // UXML 요소 찾기
        overlay = root.Q<VisualElement>("overlay");

        resumeButton = root.Q<Button>("resumeButton");
        fullscreenTextToggle = root.Q<Button>("fullscreenTextToggle");
        masterVolumeTextToggle = root.Q<Button>("masterVolumeTextToggle");

        volumeSlider = root.Q<Slider>("volumeSlider");
        volumeValueLabel = root.Q<Label>("volumeValueLabel");
        quitButton = root.Q<Button>("quitButton");

        // ====== 저장값 로드 ======
        float savedVolume = PlayerPrefs.GetFloat(PREF_VOLUME, 1f);
        masterVolumeOn = PlayerPrefs.GetInt(PREF_MASTER_ON, 1) == 1;
        isFullscreen = PlayerPrefs.GetInt(PREF_FULLSCREEN, Screen.fullScreen ? 1 : 0) == 1;

        savedVolume = Mathf.Clamp01(savedVolume);

        // ====== UI 초기값 반영 ======
        if (volumeSlider != null)
            volumeSlider.value = savedVolume;

        UpdateVolumeText(savedVolume);
        UpdateFullscreenText();
        UpdateMasterVolumeText();

        // ====== 실제 기능 적용 ======
        ApplyFullscreen(isFullscreen);
        ApplyMasterVolume(masterVolumeOn, savedVolume);

        // ====== 이벤트 연결 ======
        if (resumeButton != null)
            resumeButton.clicked += Close;

        if (fullscreenTextToggle != null)
            fullscreenTextToggle.clicked += ToggleFullscreenByText;

        if (masterVolumeTextToggle != null)
            masterVolumeTextToggle.clicked += ToggleMasterVolumeByText;

        if (volumeSlider != null)
        {
            volumeSlider.RegisterValueChangedCallback(evt =>
            {
                float v = Mathf.Clamp01(evt.newValue);
                UpdateVolumeText(v);

                // 전체볼륨 ON일 때만 실시간 반영
                if (masterVolumeOn)
                    AudioListener.volume = v;

                PlayerPrefs.SetFloat(PREF_VOLUME, v);
                PlayerPrefs.Save();
            });
        }

        if (quitButton != null)
            quitButton.clicked += QuitGame;

        // 시작은 닫힘
        SetVisible(false);
        isOpen = false;
    }

    void Update()
    {
        if (!Input.GetKeyDown(toggleKey))
            return;

        // ★ 퀘스트창이 열려있으면 설정창은 절대 열지 않음 (실행 순서 무관)
        if (QuestUIController.Instance != null && QuestUIController.Instance.IsOpen)
        {
            // (선택) 이 프레임 ESC가 다른 곳으로 새지 않게 소비 처리도 가능
            // EscapeGuard.ConsumeEsc();
            return;
        }

        // ★ 다른 UI가 ESC를 소비했으면 설정창은 열지 않음
        if (EscapeGuard.EscConsumedThisFrame())
            return;

        if (isOpen) Close();
        else Open();
    }

    public void Open()
    {
        SetVisible(true);
        isOpen = true;

        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Close()
    {
        SetVisible(false);
        isOpen = false;

        Time.timeScale = 1f;

        // 필요하면 커서 정책 원복
        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;
    }

    private void SetVisible(bool visible)
    {
        overlay.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
    }

    // =========================
    // 텍스트 토글 동작
    // =========================
    private void ToggleFullscreenByText()
    {
        isFullscreen = !isFullscreen;
        ApplyFullscreen(isFullscreen);
        UpdateFullscreenText();

        PlayerPrefs.SetInt(PREF_FULLSCREEN, isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void ToggleMasterVolumeByText()
    {
        masterVolumeOn = !masterVolumeOn;

        float v = volumeSlider != null ? Mathf.Clamp01(volumeSlider.value) : 1f;
        ApplyMasterVolume(masterVolumeOn, v);
        UpdateMasterVolumeText();

        PlayerPrefs.SetInt(PREF_MASTER_ON, masterVolumeOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    // =========================
    // 적용/표시 유틸
    // =========================
    private void ApplyFullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }

    private void ApplyMasterVolume(bool on, float sliderValue)
    {
        AudioListener.volume = on ? Mathf.Clamp01(sliderValue) : 0f;
    }

    private void UpdateFullscreenText()
    {
        if (fullscreenTextToggle == null) return;
        fullscreenTextToggle.text = isFullscreen ? "전체화면 : ON" : "전체화면 : OFF";
    }

    private void UpdateMasterVolumeText()
    {
        if (masterVolumeTextToggle == null) return;
        masterVolumeTextToggle.text = masterVolumeOn ? "전체볼륨 : ON" : "전체볼륨 : OFF";
    }

    private void UpdateVolumeText(float sliderValue)
    {
        if (volumeValueLabel == null) return;
        int pct = Mathf.RoundToInt(Mathf.Clamp01(sliderValue) * 100f);
        volumeValueLabel.text = pct + "%";
    }

    private void QuitGame()
    {
        Time.timeScale = 1f;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}