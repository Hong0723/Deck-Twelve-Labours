using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleItem3 : MonoBehaviour, IPointerClickHandler
{
    ItemData portionData;
    public Sprite usedPortionImage;
    public TMP_Text itemCount;
    public Button button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Image>().sprite = null;
        if (DeliverBattleData.BattleSceneItems[2] != null && DeliverBattleData.BattleSceneItems[2].itemName != "test")
        {
            portionData = PlayerDataToJson.Instance.CheckItemData(DeliverBattleData.BattleSceneItems[2].itemName);
            GetComponent<Image>().sprite = DeliverBattleData.BattleSceneItems[2].icon;
            itemCount.text = portionData.count.ToString();
            if (ItemType.Consumable != DeliverBattleData.BattleSceneItems[2].itemType)
            {
                DeliverBattleData.BattleSceneItems[2].useAction.Execute();
                itemCount.gameObject.SetActive(false);
            }
        }
        else
        {
            button.interactable = false;
            gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        portionData = PlayerDataToJson.Instance.CheckItemData(DeliverBattleData.BattleSceneItems[2].itemName);

        if (portionData.count == 0 || ItemType.Consumable != DeliverBattleData.BattleSceneItems[2].itemType)
        {
            return;
        }

        ItemUseAction action = DeliverBattleData.BattleSceneItems[2].useAction;

        if (action != null)
        {
            action.Execute(); 
        }

        if (portionData.count <= 1 && usedPortionImage != null)
        {
            Image portionImage = GetComponent<Image>();
            portionImage.sprite = usedPortionImage;
            itemCount.text = 0.ToString();
            button.interactable = false;
        }
        else
        {
            itemCount.text = (PlayerDataToJson.Instance.CheckItemData(portionData.name).count - 1).ToString();
        }
        PlayerDataToJson.Instance.UpdateItemData(portionData.name, -1);
    }
}