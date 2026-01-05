using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Itemslot : MonoBehaviour
{
    //슬롯안의 아이템(item)에 붙어있는 ItemManager.cs
    ItemManager item;
    
    
    void Awake()
    {
        item = GetComponentInChildren<ItemManager>(true);        
        EnrollSlot();
    }

    //InventoryManager의 리스트에 자기자신을 등록시킵니다
    //좌표값 넘겨주는 이유는 좌표값 기준으로 정렬해서
    public void EnrollSlot()
    {
        int posX = (int)transform.position.x;
        int posY = (int)transform.position.y;
        Inventory.Instance.Enroll(gameObject, posX, posY);        
    }

    public void SetItemManager(ItemManager itemManager)
    {
        item = itemManager;
    }
}
