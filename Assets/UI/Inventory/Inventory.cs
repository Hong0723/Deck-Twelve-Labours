using NUnit.Framework.Internal.Execution;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

[System.Serializable]
public class ItemSlot {
    public GameObject obj;
    public bool contained;
    public float posX;
    public float posY;
}

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    public Canvas myInventory;//인벤토리 canvas
    public bool isWatchInventory;//인벤토리 껏다켰다 filpflop
    public List<ItemSlot> itemslots;//아이템 슬롯(itembackground)의 게임오브젝트 리스트
    public static Inventory Instance { get; private set; }
    private int itemIndex;//아이템이 비어있고 제일 좌상단에 위치한 itemslots의 index
    private bool isChangeSlot;//추후 인벤토리 확장같은거 할때 true로 하면 다시 정렬하는 flag
    void Awake()
    {
        //싱글톤
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        itemslots = new List<ItemSlot>();
        isWatchInventory = false;
        itemIndex = 0;
        isChangeSlot = true;
    }

   
    void Update()
    {

        myInventory.transform.position = mainCamera.transform.position;
        //인벤토리 켜기끄기
        if (Input.GetKeyDown(KeyCode.I))
        {            
            if (isWatchInventory)
            {                
                myInventory.gameObject.SetActive(false);
                isWatchInventory = false;
            }
            else
            {
                myInventory.gameObject.SetActive(true);
                isWatchInventory = true;
            }

            //인벤토리가 켜져있고 정렬을 해야한다면
            if (isWatchInventory && isChangeSlot)
            {
                StartCoroutine(LateSort());
                isChangeSlot = false;
            }
        }
    }


    //itemslot에서 호출합니다
    //1차적으로 itemslots리스트에 itembackground오브젝트 등록
    public void Enroll(GameObject gameObject,int posx,int posy)
    {
        ItemSlot slot = new ItemSlot();
        slot.obj = gameObject;
        slot.contained = false;
        slot.posX = posx;
        slot.posY = posy;        
        itemslots.Add(slot);               
    }   

    public bool CheckImagePosition(PointerEventData eventData, GameObject gameobject)
    {
        // 현재 아이템이 들어있는 슬롯 인덱스 찾기      
        ItemManager manager = gameobject.GetComponent<ItemManager>();
        int remainIndex = manager.GetMyItemslotsIndex();
        //Debug.Log($"현재 아이템이 들어있는위치 {remainIndex}");
        if (remainIndex < 0)
        {
            
            Debug.LogError($"인덱스 {remainIndex} 기존 슬롯을 찾지 못함");
            return false;
        }
        //Debug.Log($"리스트 개수 {itemslots.Count}");
        // 모든 슬롯 순회
        for (int i = 0; i < itemslots.Count; i++)
        {
            GameObject slot = itemslots[i].obj;
            if (slot == null) continue;

            // 같은 슬롯이면 무시
            if (i == remainIndex)
                continue;
            

            RectTransform slotRect = slot.GetComponent<RectTransform>();
            if (slotRect == null) continue;

            // 슬롯 영역 안에 놓였는지 체크            
            if(RectTransformUtility.RectangleContainsScreenPoint(
                slotRect,
                eventData.position,
                eventData.pressEventCamera))
            {
                // 대상 슬롯에 이미 아이템이 있는지 확인
                GameObject remainItem = FindChildWithTag(slot.transform, "Item");
                ItemManager remainmanager = remainItem.GetComponent<ItemManager>(); 

                //Debug.Log($"옮길 자리 {i}");
                // 이미 아이템이 있고, 그게 자기 자신이 아니면
                // 여기 추후 수정해야할듯
                if (remainItem != null && remainItem != gameobject)
                {
                    GameObject remainSlot = itemslots[remainIndex].obj;
                    Transform edge2 = remainSlot.transform.Find("itemedge");
                    if (edge2 != null)
                    {
                        RectTransform rect2 = remainItem.GetComponent<RectTransform>();                        
                        rect2.SetParent(edge2, true);
                        rect2.anchoredPosition = Vector2.zero;               
                                                
                    }
                }
                // 현재 아이템을 새 슬롯으로 이동
                RectTransform rect = gameobject.GetComponent<RectTransform>();
                Transform edge = slot.transform.Find("itemedge");
                if (edge != null)
                {
                    rect.SetParent(edge, true);
                    rect.anchoredPosition = Vector2.zero;
                }

                SwapItemSlots(i, remainIndex, remainmanager, manager);
                manager.SetMyItemslotsIndex(i);
                remainmanager.SetMyItemslotsIndex(remainIndex);
                return true;
            }
        }        
        //Debug.Log("조건에 맞는 슬롯 없음");
        return false;
    }


    public void AddItem(GameObject gameObject)
    {

        //아이템창이 비어있는 최좌상단 아이템 슬롯
        for(int k=0; k<itemslots.Count;)
        {
            if (itemslots[k].contained)
            {
                continue;
            }
            else if (k + 1 >= itemslots.Count)
            {
                itemIndex = -1;//아이템창 꽉참 일단 -1
                break;
            }
            else
            {
                itemIndex = k;
                break;
            }
            
        }

        //Debug.Log($"{itemIndex}");
        //Debug.Log($"{itemslots.Count}");
        GameObject itembackground = itemslots[itemIndex].obj;
        // itemedge 찾기
        Transform itemedge = itembackground.transform.Find("itemedge");
        if (itemedge == null)
        {
            Debug.LogError("itemedge를 찾을 수 없습니다.");
            return;
        }
        // item 찾기
        Transform item = itemedge.transform.Find("Item");
        if (item == null)
        {
            Debug.LogError("오브젝트를 찾을 수 없습니다.");
            return;
        }
        // Image 컴포넌트 가져오기
        Image itemImage = item.GetComponent<Image>();
        if (itemImage == null)
        {
            Debug.LogError("오브젝트에 Image 컴포넌트가 없습니다.");
            return;
        }

        // Sprite 변경
        //지금은 맥주잔이지만 나중에 투명이미지로 바꿔서
        //아이템이 들어오면 sprite만 바꾸는 형식으로 구현
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError("월드 아이템에 SpriteRenderer가 없습니다.");
            return;
        }

        itemImage.sprite = sr.sprite;

        //ItemBase(스크립터블 오브젝트 설정
        GameObject itemGO = itemedge.Find("Item").gameObject;

        if (itemGO == null)
        {
            Debug.LogError("Item GameObject를 찾을 수 없습니다.");
            return;
        }
        ItemManager itemManager = itemGO.GetComponent<ItemManager>();

        Item itemComponent = gameObject.GetComponent<Item>();
        if (itemComponent == null)
        {
            Debug.LogError("월드 아이템에 Item 컴포넌트가 없습니다.");
            return;
        }

        itemManager.itemData = itemComponent.GetItemBase();        

        Destroy(gameObject);

        //아이템 창없으면 파괴하지말아야할듯 추후
        //슬롯 상태 갱신 추후 수정예정
        itemslots[itemIndex].contained = true;        
    }

    //itemslots리스트 내용을 서로 바꾸는 swap
    void SwapItemSlots(int indexA, int indexB, ItemManager AManager, ItemManager BManager)
    {
        Itemslot Aslot = itemslots[indexA].obj.GetComponent<Itemslot>();
        Aslot.SetItemManager(BManager);
        Itemslot Bslot = itemslots[indexB].obj.GetComponent<Itemslot>();
        Bslot.SetItemManager(AManager);             

        AManager.SetMyItemslotsIndex(indexB);
        BManager.SetMyItemslotsIndex(indexA);
    }

    GameObject FindChildWithTag(Transform parent, string tag)
    {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
        {
            if (child.CompareTag(tag))
            {
                return child.gameObject;
            }                
        }
        return null;
    }



    IEnumerator LateSort()
    {
        // 한프레임뒤 계산
        yield return null;

        itemslots.Sort((a, b) =>
        {
            // 1순위: Y (큰 값이 위)
            if (a.posY != b.posY)
            {
                return b.posY.CompareTo(a.posY);
            }

            // 2순위: X (작은 값이 왼쪽)
            return a.posX.CompareTo(b.posX);
        });

        //아이템이 가지고있는 itemManager.cs에서 자기가
        //현재 여기 itemslots리스트에 속한 인덱스를 저장합니다
        for (int i = 0; i < itemslots.Count; i++)
        {
            ItemManager item = itemslots[i].obj.GetComponentInChildren<ItemManager>();
            item.SetMyItemslotsIndex(i);
        }
    }
    
}
