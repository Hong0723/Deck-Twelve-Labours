using UnityEngine;

public class UIStateManager : MonoBehaviour
{
    [SerializeField] private GameObject optionsMenu;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool isActive = optionsMenu.activeSelf;
            optionsMenu.SetActive(!isActive);
            Time.timeScale = isActive ? 1f : 0f;
        }
    }
}
