using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler
{
  
    public RectTransform rectTransform;//아이템 Transform    
    public Camera mainCam;
    public bool isDragging = false;
    public Transform savedParent; // 임시
    public GameObject activeButton;
    public GameObject DeleteButton;
    private bool isEnterMouse;
    public Transform dragLayer; // Inspector에 DragLayer 연결 (Root Canvas 하위)
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
        isEnterMouse = false;     
        

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
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {     
        //Debug.Log("OnPointerDown");
        isDragging = true;

        if (eventData.pointerPress == activeButton)
        {
            //Debug.Log("ActiveButton 클릭됨!");
        }
        else if (eventData.pointerPress == DeleteButton)
        {
            //Debug.Log("DeleteButton 클릭됨!");
        }
        //  드래그 시작 시 버튼 숨김
        activeButton.SetActive(false);
        DeleteButton.SetActive(false);
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

        //  드래그가 끝나면 마우스가 여전히 아이템 위에 있으면 버튼 다시 표시
        if (isEnterMouse)
        {
            activeButton.SetActive(true);
            DeleteButton.SetActive(true);
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

        //  드래그 중에도 계속 비활성화 유지
        activeButton.SetActive(false);
        DeleteButton.SetActive(false);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("마우스가 들어옴!");  
        savedParent = transform.parent;//임시

        activeButton.SetActive(true);
        DeleteButton.SetActive(true);
        
        SetRaycast(activeButton, false);//아이템 위에 사용,삭제 버튼을 달아두었는데 이 버튼이 아이템의 레이캐스팅 방해해서 레이캐스팅 비활성화
        SetRaycast(DeleteButton, false);

        //아이템 설명창 표시 (ScriptableObject 기반)
        if (itemData != null)
        {
            ExplainItemToText.Instance.ShowDescription(itemData.description);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("마우스가 나감!");    

        activeButton.SetActive(false);
        DeleteButton.SetActive(false);

        ExplainItemToText.Instance.HideDescription(); //마우스 나가면 설명 숨김
    }
    private void SetRaycast(GameObject obj, bool enable)
    {
        var cg = obj.GetComponent<CanvasGroup>();
        if (cg == null) cg = obj.AddComponent<CanvasGroup>();
        cg.blocksRaycasts = enable;
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
