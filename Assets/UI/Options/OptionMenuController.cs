using UnityEngine;
using UnityEngine.UIElements;

public class OptionsMenuController : MonoBehaviour
{
    private VisualElement root;
    private Button continueButton;
    private Button soundButton;
    private Button videoButton;
    private Button quitButton;

    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        continueButton = root.Q<Button>("continue-button");
        soundButton = root.Q<Button>("sound-button");
        videoButton = root.Q<Button>("video-button");
        quitButton = root.Q<Button>("quit-button");

        continueButton.clicked += OnContinue;
        soundButton.clicked += OnSoundSettings;
        videoButton.clicked += OnVideoSettings;
        quitButton.clicked += OnQuit;
    }

    public void Show()
    {
        root.visible = true;
    }

    private void OnContinue()
    {
        Time.timeScale = 1f;
        root.visible = false;
    }

    private void OnSoundSettings()
    {
        root.visible = false;
        UIManager.Instance.ShowSoundSettings();
    }

    private void OnVideoSettings()
    {
        root.visible = false;
        UIManager.Instance.ShowVideoSettings();
    }

    private void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
