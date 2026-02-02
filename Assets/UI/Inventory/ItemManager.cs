using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
  
    public RectTransform rectTransform;//아이템 Transform    
    public Camera mainCam;
    public bool isDragging = false;
    public Transform savedParent; // 임시    
    public Transform dragLayer; // Inspector에 DragLayer 연결 (Root Canvas 하위)
    public GameObject ItemDescription;

    //ScriptableObject 아이템 데이터 연결
    [Header("Item Data")]
    public ItemBase itemData;       
    private CanvasGroup canvasGroup;
    private int myItemslotsIndex;

    void Awake()
    {
        Image img = GetComponent<Image>();
        // 완전히 투명해도 감지되게 (0은 완전 투명도까지 감지)
        img.alphaHitTestMinimumThreshold = 0.0f;        

        // CanvasGroup
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        
    }
    void Start()
    {
        RectTransform myRect = GetComponent<RectTransform>();
        
        // 부모 기준 중앙으로 이동
        myRect.anchorMin = new Vector2(0.5f, 0.5f);
        myRect.anchorMax = new Vector2(0.5f, 0.5f);
        myRect.pivot = new Vector2(0.5f, 0.5f);
        myRect.anchoredPosition = Vector2.zero;

        SetSceneCamera();
    }
    
    void SetSceneCamera()
    {
        mainCam = Inventory.Instance.GetMainCamera();
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {     
        //Debug.Log("OnPointerDown");
        isDragging = true;

        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log("OnPointerUp");
        isDragging = false;       

        bool isChangeSlot = Inventory.Instance.CheckImagePosition(eventData, gameObject);

        //Raycast 복구
        canvasGroup.blocksRaycasts = true;

        if (!isChangeSlot)
        {
            //원래위치로 복귀
            //Debug.Log("원위치 복귀");      

            RectTransform myRect = GetComponent<RectTransform>();
            // 부모 복구
            transform.SetParent(savedParent, true);
            // 부모 기준 중앙 정렬            
            myRect.anchoredPosition = Vector2.zero;
        }

        

    }

    public void OnDrag(PointerEventData eventData)
    {    
        //Debug.Log("OnDrag");
        if (!isDragging) return;

        //아이템이 마우스포인터를 따라옴
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos2;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, mousePos, mainCam, out worldPos2);
        rectTransform.position = worldPos2;

        
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("마우스가 들어옴!");  
        savedParent = transform.parent;//임시

        

        //아이템 설명창 표시 (ScriptableObject 기반)
        if (itemData != null)
        {
            ExplainItemToText.Instance.ShowDescription(itemData.description);
        }

        
        //새로운 설명창        

        RectTransform rect = ItemDescription.GetComponent<RectTransform>();

        // 좌상단 기준
        rect.pivot = new Vector2(0f, 1f);

        //마우스위치를 월드좌표로 변경
        Vector2 WorldPos;
        WorldPos = mainCam.ScreenToWorldPoint(eventData.position);       

        // 좌상단이 마우스 위치에 오도록
        rect.position = WorldPos;

        //rect.SetParent(dragLayer, true);

        ItemDescription.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("마우스가 나감!");    

       

        ExplainItemToText.Instance.HideDescription(); //마우스 나가면 설명 숨김

        ItemDescription.SetActive(false);

    }

    
    public void OnBeginDrag(PointerEventData eventData)
    {
        
        if (dragLayer == null)
            dragLayer = GameObject.Find("DragLayer").transform;
            
        // DragLayer로 이동
        transform.SetParent(dragLayer, true);
        // 드래그 중엔 아이템이 Raycast를 막지 않도록
        canvasGroup.blocksRaycasts = false;
        // (선택) 아이템을 가장 마지막으로 (맨 위에) 보이게
        transform.SetAsLastSibling();
    }
    public void OnEndDrag(PointerEventData eventData)
    {        
        // Raycast 다시 활성화
        Vector3 releasedPos = transform.position;
        //Debug.Log("releasedPos: " + releasedPos);  
        canvasGroup.blocksRaycasts = true;
    }

    // 아이템 사용
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("우클릭 감지!");
            Image itemImage = GetComponent<Image>();
            itemImage.sprite = null;
            itemImage.color = new Color(1, 1, 1, 0);

            if (itemData.useAction == null)
            {
                Debug.Log("사용할 수 없는 아이템");
                return;
            }

            //아이템 사용
            itemData.useAction.Execute(gameObject);

            //아이템 사용후 슬롯 재정비 미구현
            //ConsumeItem();

        }
    }

    //아이템 충돌이후 아이템을 인벤토리에 등록
    public void ItemToInventory()
    {
        Inventory.Instance.AddItem(gameObject);
    }

    public void SetMyItemslotsIndex(int index)
    {
        myItemslotsIndex = index;
    }

    public int GetMyItemslotsIndex()
    {
        return myItemslotsIndex;
    }

    
}
