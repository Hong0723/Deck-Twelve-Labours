using NUnit.Framework.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleItem1 : MonoBehaviour, IPointerClickHandler
{
    ItemData portionData;//아이템 정보
    public Sprite usedPortionImage;//아이템 다 사용한 경우 나타낼 sprite
    public TMP_Text itemCount;//아이템 개수표시 text
    public Button button;//실행버튼
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Image>().sprite = null;
        if (DeliverBattleData.BattleSceneItems[0] != null && DeliverBattleData.BattleSceneItems[0].itemName != "test")//test는 아이템이 없음을 나타내는 스크립터블 오브젝트
        {
            portionData = PlayerDataToJson.Instance.CheckItemData(DeliverBattleData.BattleSceneItems[0].itemName);
            GetComponent<Image>().sprite = DeliverBattleData.BattleSceneItems[0].icon;            
            itemCount.text = portionData.count.ToString();

            //소모품 이외의 아이템 경우 배틀 시작전 미리 사용하여 플레이어에게 능력치 적용
            //ex 검(공격력 5추가)
            if (ItemType.Consumable != DeliverBattleData.BattleSceneItems[0].itemType)
            {
                DeliverBattleData.BattleSceneItems[0].useAction.Execute();
                itemCount.gameObject.SetActive(false);
            }
        }
        else//아이템 없음
        {
            button.interactable = false;
            gameObject.SetActive(false);
        }        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        portionData = PlayerDataToJson.Instance.CheckItemData(DeliverBattleData.BattleSceneItems[0].itemName);
        if (portionData.count == 0 || ItemType.Consumable != DeliverBattleData.BattleSceneItems[0].itemType)
        {
            return;
        }

        //아이템 스크립터블 오브젝트에 저장된 동작실행
        ItemUseAction action = DeliverBattleData.BattleSceneItems[0].useAction;
        if (action != null)
        {
            action.Execute();
        }

        //마지막 1개 소모품 사용할때
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
