using UnityEngine;

public class Item : MonoBehaviour
{    
    public ItemBase myBase;
    [Header("Pickup SFX")]
    [SerializeField] private AudioClip pickupSFX;
    [SerializeField] private float pickupVolume = 0.7f;

    //씬이 로드되었을때 이전에 주은 아이템이면 비활성화
    void Start()
    {
        if (GlobalItemState.IsPicked(myBase.itemName, myBase.ItemID))
        {
            gameObject.SetActive(false);
        }
    }

    //������ ȹ��
    public void ItemToInventory()
    {
        if (pickupSFX != null && SFXManager.Instance != null)
        {
        SFXManager.Instance.PlaySFX(pickupSFX, pickupVolume);
        }
        if (Inventory.Instance != null)
        {
            Inventory.Instance.AddItem(gameObject);
            GlobalItemState.MarkAsPicked(myBase.itemName, myBase.ItemID);
        }
        else
        {
            Debug.LogError("Inventory.Instance�� null�Դϴ�!");
        }
    }

    public ItemBase GetItemBase()
    {
        return myBase;
    }
}
