using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private ItemBase myBase;
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
