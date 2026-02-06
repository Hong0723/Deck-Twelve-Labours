using NUnit.Framework.Interfaces;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Itemslot : MonoBehaviour
{
    //슬롯안의 아이템(item)에 붙어있는 ItemManager.cs
    private ItemManager item;
    [SerializeField]
    private bool isstand = false;
    private bool isFirstEffect;
    [SerializeField]
    private Animator animator;
    public Image itemEdgeImg;


    void Awake()
    {
        item = GetComponentInChildren<ItemManager>(true);        
        EnrollSlot();
        isFirstEffect = true;
    }

    //InventoryManager의 리스트에 자기자신을 등록시킵니다
    //좌표값 넘겨주는 이유는 좌표값 기준으로 정렬해서
    public void EnrollSlot()
    {
        RectTransform rt = transform as RectTransform;
        Vector2 pos = rt.anchoredPosition;

        int posX = (int)pos.x;
        int posY = (int)pos.y;
        Inventory.Instance.Enroll(gameObject, posX, posY, isstand);        
    }

    public void SetItemManager(ItemManager itemManager)
    {
        item = itemManager;

       
    }

    public void CreateEffectActivate()
    {
        if (isFirstEffect)
        {
            StartCoroutine(CreateEffect());
            isFirstEffect = false;
        }
        
    }

    public void SetItemEdgeAlpha(int alpha)
    {
        Transform edgeTr = transform.Find("itemedge");
        itemEdgeImg = edgeTr.GetComponent<Image>();
        Color c = itemEdgeImg.color;
        c.a = alpha;      // 0 = 투명, 1 = 불투명
        itemEdgeImg.color = c;
    }

    IEnumerator CreateEffect()
    {
        
        animator.SetTrigger("CreateEffect");
        Debug.Log("이펙트 발동");

        yield return new WaitForSeconds(1.0f);

        SetItemEdgeAlpha(1);
    }
}
