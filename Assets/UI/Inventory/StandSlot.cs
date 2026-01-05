using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class StandSlot : MonoBehaviour
{
    //슬롯안의 아이템(item)에 붙어있는 ItemManager.cs
    ItemManager item;

    void Awake()
    {
        item = GetComponentInChildren<ItemManager>(true);
        
    }

    
}
