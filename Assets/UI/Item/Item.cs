using UnityEngine;

public class Item : MonoBehaviour
{
    //��ũ���ͺ� ������Ʈ
    public ItemBase myBase;
    [Header("Pickup SFX")]
    [SerializeField] private AudioClip pickupSFX;
    [SerializeField] private float pickupVolume = 0.7f;


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
