using UnityEngine;
using UnityEngine.UIElements;

public class InventoryMenuController : MonoBehaviour
{
    private VisualElement root;

    void OnEnable()
    {
        var uiDoc = GetComponent<UIDocument>();
        root = uiDoc.rootVisualElement;
        root.visible = false; // 시작 시 비활성화
    }

    public void ToggleInventory()
    {
        root.visible = !root.visible;
    }
}
