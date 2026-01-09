using UnityEngine;
using UnityEngine.UIElements;

public class SoundSettingsMenuController : MonoBehaviour
{
    public static SoundSettingsMenuController Instance;
    private VisualElement root;

    private Slider bgmSlider;
    private Slider sfxSlider;

    private Label bgmPercent;
    private Label sfxPercent;

    private Button backButton;

    void Awake() => Instance = this;
    
    void OnEnable()
    {
        var ui = GetComponent<UIDocument>();
        root = ui.rootVisualElement;
        root.visible = false;

        bgmSlider = root.Q<Slider>("bgm-slider");
        sfxSlider = root.Q<Slider>("sfx-slider");

        bgmPercent = root.Q<Label>("bgm-percent");
        sfxPercent = root.Q<Label>("sfx-percent");

        backButton = root.Q<Button>("back-button");

        bgmSlider.RegisterValueChangedCallback(evt => UpdateBgm(evt.newValue));
        sfxSlider.RegisterValueChangedCallback(evt => UpdateSfx(evt.newValue));

        backButton.clicked += () => {
            root.visible = false;
            UIManager.Instance.ShowOptions();
        };

        RefreshAll();
    }

    private void RefreshAll()
    {
        UpdateBgm(bgmSlider.value);
        UpdateSfx(sfxSlider.value);
    }

    private void UpdateBgm(float v)
    {
        bgmPercent.text = Mathf.RoundToInt(v * 100f) + "%";
    }

    private void UpdateSfx(float v)
    {
        sfxPercent.text = Mathf.RoundToInt(v * 100f) + "%";
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
