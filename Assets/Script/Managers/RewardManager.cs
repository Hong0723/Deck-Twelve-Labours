using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class RewardManager : MonoBehaviour,
    IPointerClickHandler
{
    public GameObject RewardItem1;
    public GameObject RewardItem2;

    public List<GameObject> RewardTexts;

    ItemBase ScriptableItem1;
    ItemBase ScriptableItem2;

    private GameObject CurrentObject;//이전에 선택한 highlight오브젝트

    public List<ItemBase> allItems;//보상 아이템 스크립터블 오브젝트 목록
    float total;
    float delay = 0.3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        total = 0f;

        foreach (var d in allItems)
            total += d.dropRate;


        ScriptableItem1 = GetRandomItem();
        ScriptableItem2 = GetRandomItem();

        RewardItem1.GetComponent<Image>().sprite = ScriptableItem1.icon;
        RewardItem2.GetComponent<Image>().sprite = ScriptableItem2.icon;

        RewardItem1.GetComponentInChildren<TextMeshProUGUI>().text = ScriptableItem1.itemName;
        RewardItem2.GetComponentInChildren<TextMeshProUGUI>().text = ScriptableItem2.itemName;


        StartCoroutine(PlaySequential());
    }

    IEnumerator PlaySequential()
    {
        foreach (var text in RewardTexts)
        {
            text.GetComponent<Animator>().SetBool("Play", true);
            yield return new WaitForSeconds(delay);
        }
    }

    

    // 클릭 판정 (Down + Up 같은 오브젝트)
    public void OnPointerClick(PointerEventData eventData)
    {       
        GameObject targetObject = eventData.pointerEnter;
        Transform highlight = targetObject.transform.Find("highlight");
        if (highlight == null)
        {
            Debug.Log("highlight 없음 : " + targetObject.name);
            return;
        }        
        GameObject targetHighlight = highlight.gameObject;

        //이전에 선택한 오브젝트 하이라이트 제거
        if (CurrentObject != null)
        {
            Transform Currenthighlight =
            CurrentObject.transform.Find("highlight");
            Currenthighlight.gameObject.SetActive(false);
            Image img = CurrentObject.GetComponent<Image>();
            Color c = img.color;
            c.a = 1f;
            img.color = c;
        }
        //선택한 오브젝트 갱신
        CurrentObject = targetObject;

        targetHighlight.SetActive(true);        
        {
            Image img = targetObject.GetComponent<Image>();
            Color c = img.color;
            c.a = 0.3f;
            img.color = c;
        }           
    }

    //Continue 버튼 클릭으로 실행
    public void AddItemToInventory()
    {
        //인벤토리에 아이템 추가
        if (CurrentObject == RewardItem1 || CurrentObject.transform.IsChildOf(RewardItem1.transform))
        {
            Inventory.Instance.AddItem(ScriptableItem1);
        }
        else if (CurrentObject == RewardItem2 || CurrentObject.transform.IsChildOf(RewardItem2.transform))
        {
            Inventory.Instance.AddItem(ScriptableItem2);
        }
        else
        {
            Debug.Log("아이템이 없음");
        }
    }

    public ItemBase GetRandomItem()
    {      
        float rand = Random.Range(0, total);
        float current = 0f;

        foreach (var d in allItems)
        {
            current += d.dropRate;

            if (rand <= current)
                return d;
        }

        return null;
    }
}
