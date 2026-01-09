using UnityEditor.Rendering;
using UnityEngine;

public class ItemCollisionCheck : MonoBehaviour
{
    
    //아이템과 부딪히면 충돌감지
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            Item item = other.GetComponent<Item>();
            if (item != null)
            {
                item.ItemToInventory();
                //Debug.Log("Item 태그와 충돌함");
            }
        }
    }


}
