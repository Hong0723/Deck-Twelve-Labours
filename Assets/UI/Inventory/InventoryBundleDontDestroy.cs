using UnityEngine;

public class InventoryBundleDontDestroy : MonoBehaviour
{
    private static InventoryBundleDontDestroy instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject); // 중복 생성 방지
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}