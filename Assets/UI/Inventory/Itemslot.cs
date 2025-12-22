using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Itemslot : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        EnrollSlot();
    }   

    //자기자신을 Inventory스크립트의 List에 등록
    public void EnrollSlot()
    {
        Inventory.Instance.Enroll(gameObject);
    }
}
