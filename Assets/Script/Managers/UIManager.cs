using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public OptionsMenuController optionsMenu;
    public SoundSettingsMenuController soundMenu;
    public VideoSettingsMenuController videoMenu;

    void Awake()
    {
        Instance = this;
    }
    
    public void ShowOptions()
    {
        optionsMenu.Show();
    }

    public void ShowSoundSettings()
    {
        soundMenu.Show();
    }

    public void ShowVideoSettings()
    {
        videoMenu.Show();
    }
}
