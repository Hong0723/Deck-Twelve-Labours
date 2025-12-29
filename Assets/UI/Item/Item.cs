using UnityEngine;

public class Item : MonoBehaviour
{
    //스크립터블 오브젝트
    [SerializeField]
    private ItemBase myBase;

    //아이템 획득
    public void ItemToInventory()
    {
        if (Inventory.Instance != null)
        {
            Inventory.Instance.AddItem(gameObject);
        }
        else
        {
            Debug.LogError("Inventory.Instance가 null입니다!");
        }
    }

    public ItemBase GetItemBase()
    {
        return myBase;
    }
}
