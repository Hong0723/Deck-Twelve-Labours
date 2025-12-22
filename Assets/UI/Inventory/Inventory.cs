using NUnit.Framework.Internal.Execution;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class ItemSlot {
    public GameObject obj;
    public bool contained;
}



public class Inventory : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    public Canvas myInventory;//인벤토리 canvas
    public bool isWatchInventory;//인벤토리 껏다켰다
    public List<ItemSlot> itemslots;//아이템 슬롯(배경)의 게임오브젝트 리스트
    public static Inventory Instance { get; private set; }
    private int itemIndex;//아이템창이 비어있는 가장 낮은 index
    void Awake()
    {

        //싱글톤
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 씬 이동 시에도 유지 (원하면 삭제 가능)


        itemslots = new List<ItemSlot>();

        isWatchInventory = false;
        itemIndex = 0;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

        }
    }


    //itemslot에서 호출합니다
    public void Enroll(GameObject gameObject)
    {
        ItemSlot slot = new ItemSlot();
        slot.obj = gameObject;
        slot.contained = false;

        itemslots.Add(slot);
    }

    public bool CheckImagePosition(Vector3 releasedPos, GameObject gameobject)
    {
        foreach (ItemSlot itmeSlot in itemslots) {

            GameObject slot = itmeSlot.obj;
            Collider col = slot.GetComponent<Collider>();
            if (col == null)
                continue;

            // Collider2D의 Bounds 영역 안에 있는지 체크
            //아이템 슬롯의 범위내에 아이템을 놔두었을 경우 알아서 중앙에 배치되도록 하려고
            if (col.bounds.Contains(releasedPos))
            {
                Debug.Log($" {slot.name} 슬롯 안에 아이템이 놓였습니다!");
                RectTransform rect = gameobject.GetComponent<RectTransform>();

                /*
                RectTransform slotRect = slot.GetComponent<RectTransform>();
                // 부모를 슬롯으로 변경
                rect.SetParent(slotRect, worldPositionStays: false);
                */
                // slot(itembackground)의 자식 중 itemedge 찾기

                Transform edge = slot.transform.Find("itemedge");
                if (edge == null)
                {
                    Debug.LogError($"{slot.name} 안에 itemedge가 없습니다! 확인해주세요.");
                    return false;
                }

                // 부모를 itemedge로 변경
                rect.SetParent(edge, worldPositionStays: true);


                // 로컬 위치를 0으로 맞추면 슬롯 중앙에 정렬됨
                rect.anchoredPosition = Vector2.zero;

                rect.position = slot.transform.position;//11.13
                return true;
            }
        }
        Debug.Log($"Item 오브젝트가 놓인 위치: {releasedPos}");
        return false;
    }

    public void AddItem(GameObject gameObject)
    {
        //Debug.Log(itemIndex);
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
        //Destroy(itemGO);


        // 슬롯 상태 갱신 추후 수정예정
        itemslots[itemIndex].contained = true;
        itemIndex++;
    }
}
