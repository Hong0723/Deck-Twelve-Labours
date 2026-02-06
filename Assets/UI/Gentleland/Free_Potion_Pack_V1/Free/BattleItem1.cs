using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleItem1 : MonoBehaviour, IPointerClickHandler
{
    ItemData portionData;
    public Sprite usedPortionImage;
    public TMP_Text itemCount;
    public Button button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (DeliverBattleData.BattleSceneItems[0] != null)
        {
            portionData = PlayerDataToJson.Instance.CheckItemData(DeliverBattleData.BattleSceneItems[0].itemName);
            GetComponent<Image>().sprite = DeliverBattleData.BattleSceneItems[0].icon;
            //GetComponent<TMP_Text>().text = portionData.count.ToString();
            itemCount.text = portionData.count.ToString();
        }
        else
        {
            button.interactable = false;
            gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        portionData = PlayerDataToJson.Instance.CheckItemData(DeliverBattleData.BattleSceneItems[0].itemName);
        if (portionData.count == 0)
        {
            return;
        }

        ItemUseAction action = DeliverBattleData.BattleSceneItems[0].useAction;

        if (action != null)
        {
            action.Execute(gameObject); // 메서드명 대문자 권장
        }


        if (portionData.count <= 1 && usedPortionImage!=null)
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
