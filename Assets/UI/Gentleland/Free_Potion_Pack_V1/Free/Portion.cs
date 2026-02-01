using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Portion : MonoBehaviour, IPointerClickHandler
{
    //public int portionCount;
    public Sprite usePortionImage;
    public TMP_Text itemCount;
    public Button button;
    public string itemName;
    public void OnPointerClick(PointerEventData eventData)
    {
        
        ItemData portionData = PlayerDataToJson.Instance.CheckItemData(itemName);

        if (portionData.count == 0)
        {
            return;
        }

        if (portionData.count <= 1)
        {
            Image portionImage = GetComponent<Image>();
            portionImage.sprite = usePortionImage;
            itemCount.text = 0.ToString();
            button.interactable = false;
        }
        else
        {
            itemCount.text= (PlayerDataToJson.Instance.CheckItemData(itemName).count-1).ToString();
        }
        PlayerDataToJson.Instance.UpdateItemData(itemName, -1);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        itemCount.text = PlayerDataToJson.Instance.CheckItemData(itemName).count.ToString();
        if (PlayerDataToJson.Instance.CheckItemData(itemName).count > 0)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
        
    }

    
}
