using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal.Execution;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Progress;

[System.Serializable]
public class ItemSlot {
    public GameObject obj;//itembacground오브젝트
    public bool contained;//obj에 아이템 유무
    public float posX;//itembacground 좌표
    public float posY;
    public bool isStand;//battleScene으로 아이템 연결 유무(true->배틀씬으로 연결)
}

public class Inventory : MonoBehaviour
{
    //[SerializeField]
    public Camera mainCamera;
    public Canvas myInventory;//인벤토리 canvas
    public bool isWatchInventory;//인벤토리 껏다켰다 filpflop
    public List<ItemSlot> itemslots;//아이템 슬롯(itembackground)의 게임오브젝트 리스트
    public static Inventory Instance { get; private set; }
    private int itemIndex;//아이템이 비어있고 제일 좌상단에 위치한 itemslots의 index
    private bool isChangeSlot;//추후 인벤토리 확장같은거 할때 true로 하면 다시 정렬하는 flag
    private bool firstSort;
    private List<List<ItemSlot>> grid;
    [SerializeField]
    private GameObject slotPrefab;
    public GameObject Content;

    [SerializeField]
    private GraphicRaycaster fogRaycaster;//인벤토리 활성화 되있을때는 off        

    void Awake()
    {
        //싱글톤
        if (Instance != null)
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
        firstSort = false;

        grid = new List<List<ItemSlot>>();
        grid.Add(new List<ItemSlot>(4));

        //초기화후 바로 비활성화
        myInventory.gameObject.SetActive(true);
        StartCoroutine(LateSort());
        myInventory.gameObject.SetActive(false);        
    }
    
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //씬 바뀌었을때 mainCamera 재할당
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        //임시로
        if(myInventory != null)
        {            
            float distance = 5f; // 카메라 앞 거리
            myInventory.transform.position =
                mainCamera.transform.position +
                mainCamera.transform.forward * distance;                        
            myInventory.transform.rotation = mainCamera.transform.rotation;
        }
        
        //인벤토리 켜기끄기
        if (Input.GetKeyDown(KeyCode.I))
        {            
            if (isWatchInventory)
            {                
                myInventory.gameObject.SetActive(false);
                isWatchInventory = false;
                fogRaycaster.enabled = true;
            }
            else
            {
                myInventory.gameObject.SetActive(true);
                isWatchInventory = true;
                fogRaycaster.enabled = false;
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
    public void Enroll(GameObject gameObject, int posx, int posy, bool isstand)
    {
        ItemSlot slot = CreateItemSlotObj(gameObject, posx, posy, isstand);
        itemslots.Add(slot);               
    }   

    public ItemSlot CreateItemSlotObj(GameObject gameObject, int posx, int posy, bool isstand)
    {
        ItemSlot slot = new ItemSlot();
        slot.obj = gameObject;
        slot.contained = false;
        slot.posX = posx;
        slot.posY = posy;
        slot.isStand = isstand;
        return slot;
    }

    public bool CheckImagePosition(PointerEventData eventData, GameObject gameobject)
    {
        // 현재 아이템이 들어있는 슬롯 인덱스 찾기      
        ItemManager ToItemManager = gameobject.GetComponent<ItemManager>();
        int ToItemSlotIndex = ToItemManager.GetMyItemslotsIndex();
        
        if (ToItemSlotIndex < 0)
        {            
            Debug.LogError($"인덱스 {ToItemSlotIndex} 기존 슬롯을 찾지 못함");
            return false;
        }
        
        // 모든 슬롯 순회
        for (int i = 0; i < itemslots.Count; i++)
        {
            GameObject slot = itemslots[i].obj;
            if (slot == null) 
                continue;
           
            // 같은 슬롯이면 무시
            if (i == ToItemSlotIndex)
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
                GameObject FromItem = FindChildWithTag(slot.transform, "Item");
                ItemManager FromItemManager = FromItem.GetComponent<ItemManager>(); 
                
                // 이미 아이템이 있고, 그게 자기 자신이 아니면
                if (FromItem != null && FromItem != gameobject)
                {
                    GameObject remainSlot = itemslots[ToItemSlotIndex].obj;//드래그 시작지점 itembackground
                    Transform FromitemEdgeTransform = remainSlot.transform.Find("itemedge");
                    if (FromitemEdgeTransform != null)
                    {
                        RectTransform FromitemTrasform = FromItem.GetComponent<RectTransform>();                        
                        FromitemTrasform.SetParent(FromitemEdgeTransform, true);
                        FromitemTrasform.anchoredPosition = Vector2.zero;               
                                                
                    }
                }
                // 현재 아이템을 새 슬롯으로 이동
                RectTransform ToItemTrasform = gameobject.GetComponent<RectTransform>();
                Transform ToitemedgeTransform = slot.transform.Find("itemedge");
                if (ToitemedgeTransform != null)
                {
                    ToItemTrasform.SetParent(ToitemedgeTransform, true);
                    ToItemTrasform.anchoredPosition = Vector2.zero;
                }                

                SwapItemSlots(i, ToItemSlotIndex, FromItemManager, ToItemManager);                

                ToItemManager.SetMyItemslotsIndex(i);
                FromItemManager.SetMyItemslotsIndex(ToItemSlotIndex);


                //itemslots배열 contained 갱신
                //test이름의 스크립터블 오브젝트가 있는 곳이 아이템 빈칸으로 -> contained = false
                int index = 0;
                foreach (var slotData in itemslots)
                {
                    if (slotData.isStand)
                        continue;

                    // 슬롯에 연결된 ItemManager 찾기
                    ItemManager manager =
                        slotData.obj.GetComponentInChildren<ItemManager>();

                    if (manager == null || manager.itemData == null)
                    {
                        slotData.contained = false;
                    }
                    else if (manager.itemData.itemName == "test")
                    {
                        slotData.contained = false;
                    }
                    else
                    {
                        slotData.contained = true;
                    }

                    Debug.Log($"[{index}] contained = {slotData.contained}");
                    index++;
                }

                return true;
            }
        }                
        return false;
    }


    public void AddItem(GameObject NewItemObject)
    {
        bool isDone = false;

        //기존 아이템과 같은 아이템이 추가될때 수량만 변경
        for (int k = 0; k < itemslots.Count; k++)
        {
            ItemManager manager = itemslots[k].obj.GetComponentInChildren<ItemManager>();
            ItemBase ItemScriptableObject = manager.itemData;//기존 아이템
            ItemBase NewItemScriptableObject = NewItemObject.GetComponent<Item>().myBase;//새로 추가한 아이템
            if (ItemScriptableObject && ItemScriptableObject.itemName == NewItemScriptableObject.itemName)
            {
                //Json 파일 업데이트
                GetComponent<PlayerDataToJson>().UpdateItemData(NewItemScriptableObject.itemName, 1);                
                manager.SetItemAmount();
                Destroy(NewItemObject);
                return;
            }
        }

        if (isDone)
        {
            isDone = false;
            return;
        }

        //기존 아이템과 같은 아이템이 없을때
        //아이템창이 비어있는 최좌상단 아이템 슬롯
        for (int k=0; k<itemslots.Count; k++)
        {
            //석상에는 넣지 않습니다            
           if (itemslots[k].isStand)
           {
               continue;
           }
           
           //아이템이 이미 존재
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

        GameObject ToItembackground = itemslots[itemIndex].obj;
        // itemedge 찾기
        Transform ToItemEdge = ToItembackground.transform.Find("itemedge");
        if (ToItemEdge == null)
        {
            Debug.LogError("itemedge를 찾을 수 없습니다.");
            return;
        }
        // item 찾기
        Transform ToItem = ToItemEdge.transform.Find("Item");
        if (ToItem == null)
        {
            Debug.LogError("오브젝트를 찾을 수 없습니다.");
            return;
        }
        // Image 컴포넌트 가져오기
        Image BackgroundImage = ToItem.GetComponent<Image>();
        if (BackgroundImage == null)
        {
            Debug.LogError("오브젝트에 Image 컴포넌트가 없습니다.");
            return;
        }

                

        //ItemBase(스크립터블 오브젝트 설정
        GameObject FromItemObj = ToItemEdge.Find("Item").gameObject;

        if (FromItemObj == null)
        {
            Debug.LogError("Item GameObject를 찾을 수 없습니다.");
            return;
        }
        ItemManager FromItemManager = FromItemObj.GetComponent<ItemManager>();

        Item ItemInfoScriptableObj = NewItemObject.GetComponent<Item>();
        if (ItemInfoScriptableObj == null)
        {
            Debug.LogError("월드 아이템에 Item 컴포넌트가 없습니다.");
            return;
        }

        //스크립터블 오브젝트교체                      
        FromItemManager.SetItemData(ItemInfoScriptableObj.GetItemBase());
        // Sprite 변경        
        //아이템이 들어오면 sprite만 바꾸는 형식으로 구현
        BackgroundImage.sprite = ItemInfoScriptableObj.GetItemBase().icon;
        BackgroundImage.color = new Color(1, 1, 1, 1);


        //스크립터블오브젝트의 이름가져오기위해
        ItemBase ItemScriptable = ItemInfoScriptableObj.GetItemBase();
        GetComponent<PlayerDataToJson>().UpdateItemData(ItemScriptable.itemName, 1);
        FromItemManager.SetItemAmount();
        
        Destroy(NewItemObject);

        //아이템이 들어갈 창없으면 파괴x->미구현
        //위 상태일때 슬롯 상태 갱신->미구현
        itemslots[itemIndex].contained = true;        
    }

    //인벤토리에 아이템 등록하려고 할때 스크립터블 오브젝트만 넘겨서 쓰는코드
    public void AddItem(ItemBase scriptableobject)
    {
        GameObject obj = new GameObject();
        Item comp = obj.AddComponent<Item>();
        comp.myBase = scriptableobject; // ScriptableObject 참조만 연결
        AddItem(obj);        
    }

    //itemslots리스트 내용을 서로 바꾸는 swap
    void SwapItemSlots(int indexA, int indexB, ItemManager AManager, ItemManager BManager)
    {       
        Itemslot Aslot = itemslots[indexA].obj.GetComponent<Itemslot>();
        Aslot.SetItemManager(BManager);
        Itemslot Bslot = itemslots[indexB].obj.GetComponent<Itemslot>();
        Bslot.SetItemManager(AManager);

        //BattleSceneItem스크립트가 아이템 슬롯이 붙어있는 오브젝트이면 BattleScene으로 연결
        BattleSceneItem InStandItem = Aslot.GetComponentInChildren<BattleSceneItem>();
        if (InStandItem != null)
        {
            ItemBase itemData = BManager.itemData;            
            //test는 빈 아이템
            if (itemData == null || itemData.itemName == "test")
            {
                InStandItem.ClearStaticItemData();//BattleScene과 연결되는 static변수 초기화               
            }
            else
            {
                InStandItem.SetStaticItemData(itemData);//BattleScene과 연결되는 static변수로 전달     
            }

            
        }
            BattleSceneItem InStandItem2 = Bslot.GetComponentInChildren<BattleSceneItem>();
            if (InStandItem2 != null)
            {
                ItemBase itemData2 = AManager.itemData;
                
            if (itemData2 == null || itemData2.itemName == "test")
            {
                InStandItem2.ClearStaticItemData();                
            }
            else
            {
                InStandItem2.SetStaticItemData(itemData2);                
            }
            
        }
        
        AManager.SetMyItemslotsIndex(indexB);
        BManager.SetMyItemslotsIndex(indexA);       
    }

    //parent transform 하위의 오브젝트 중에 tag검색
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

        Debug.Log("Sort함");
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
            ItemManager itemManager = itemslots[i].obj.GetComponentInChildren<ItemManager>();
            itemManager.SetMyItemslotsIndex(i);
        }
    }

    public void gridListCreate()
    {
        Debug.Log(itemslots.Count); 
        Debug.Log("gridListCreate");

        for (int i = 0; i < itemslots.Count; i++)
        {
            if (itemslots[i].isStand)
                continue;

            // 마지막 행이 꽉 찼으면 새 행 추가
            if (grid[grid.Count - 1].Count >= 4)
            {
                grid.Add(new List<ItemSlot>(4));
            }

            grid[grid.Count - 1].Add(itemslots[i]);
        }
        //디버그
        for (int r = 0; r < grid.Count; r++)
        {
            string rowLog = $"Row {r}: ";
            for (int c = 0; c < grid[r].Count; c++)
            {
                rowLog += $"[{c}] ";
            }
            Debug.Log(rowLog);
        }
        //디버그

    }
    //content y 좌표가 -150에서 150
    //처음 30 띄우고 시작 간격 50씩
    //x좌표는 -90 ~90 처음 20띄우고 간격 47씩
    //사과아이템을 위한 함수(인벤토리 확장 아이템)
    public void AddInventorySlot(int amount)//생성할 개수
    {
        if (!firstSort)
        {
            gridListCreate();
            firstSort = true;
        }
        



        int index = 0;
        for (int i = 0; i < amount; i++)
        {

            Debug.Log(grid[grid.Count - 1].Count);
            //한 행에 4개씩 아이템 슬롯을 배치할건데 현재 행에 슬롯이 4개보다 작은경우
            if (grid[grid.Count - 1].Count >= 4)
            {
                
                grid.Add(new List<ItemSlot>(4)); // 열은 항상 4
                
                float xpos = grid[grid.Count - 2][0].posX;
                float ypos = grid[grid.Count - 2][0].posY - 50;
                Debug.Log("xpos = " + xpos + ", ypos = " + (ypos+50));

                // 1️ 위치 없이 생성 (UI는 위치 나중에 세팅)
                GameObject slotObj = Instantiate(slotPrefab, Content.transform);
                
                // 2️ RectTransform 가져오기
                RectTransform slotTransform = slotObj.GetComponent<RectTransform>();

                // 3️ anchoredPosition으로 위치 지정
                slotTransform.anchoredPosition = new Vector2(xpos, ypos);

                // 생성되는 효과발생
                Itemslot EffectSlot = slotObj.GetComponent<Itemslot>();
                EffectSlot.SetItemEdgeAlpha(0);
                EffectSlot.CreateEffectActivate();

                grid[grid.Count - 1].Add(CreateItemSlotObj(slotObj, (int)xpos, (int)ypos, false));
                Debug.Log("xpos = " + xpos + ", ypos = " + ypos + ", spawnPos = " + slotTransform.anchoredPosition);
                //index++;
                //디버그
                for (int r = 0; r < grid.Count; r++)
                {
                    string rowLog = $"Row {r}: ";
                    for (int c = 0; c < grid[r].Count; c++)
                    {
                        rowLog += $"[{c}] ";
                    }
                    Debug.Log(rowLog);
                }
                //디버그
            }
            else
            {
                index = grid[grid.Count-1].Count-1;
                float xpos = grid[grid.Count - 1][index].posX + 47;
                float ypos = grid[grid.Count - 1][index].posY;

                // 1️ 위치 없이 생성 (UI는 위치 나중에 세팅)
                GameObject slot = Instantiate(slotPrefab, Content.transform);

                // 2️ RectTransform 가져오기
                RectTransform rt = slot.GetComponent<RectTransform>();

                // 3️ anchoredPosition으로 위치 지정
                rt.anchoredPosition = new Vector2(xpos, ypos);

                // 생성되는 효과발생
                Itemslot EffectSlot = slot.GetComponent<Itemslot>();
                EffectSlot.SetItemEdgeAlpha(0);
                EffectSlot.CreateEffectActivate();

                grid[grid.Count - 1].Add(CreateItemSlotObj(slot, (int)xpos, (int)ypos, false));
                Debug.Log("xpos = " + xpos + ", ypos = " + ypos + ", spawnPos = " + rt.anchoredPosition);
                //디버그
                for (int r = 0; r < grid.Count; r++)
                {
                    string rowLog = $"Row {r}: ";
                    for (int c = 0; c < grid[r].Count; c++)
                    {
                        rowLog += $"[{c}] ";
                    }
                    Debug.Log(rowLog);
                }
                //디버그
            }
        }
        //sort호출용
        isChangeSlot = true;

    }

    public void ResetItemSlot(PointerEventData eventData)
    {
        // 모든 슬롯 순회
        for (int i = 0; i < itemslots.Count; i++)
        {
            GameObject slot = itemslots[i].obj;
            if (slot == null)
                continue;


            RectTransform slotRect = slot.GetComponent<RectTransform>();
            if (slotRect == null) continue;

            // 슬롯 영역 안에 놓였는지 체크            
            if (RectTransformUtility.RectangleContainsScreenPoint(
                slotRect,
                eventData.position,
                eventData.pressEventCamera))
            {
                itemslots[i].contained = false;
                TMP_Text textCanvas = itemslots[i].obj.GetComponentInChildren<TMP_Text>();
                if (textCanvas != null)
                {
                    textCanvas.gameObject.SetActive(false);
                }
            }
        }
    }
}
