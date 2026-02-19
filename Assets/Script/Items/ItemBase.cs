using UnityEngine;

public enum ItemType
{
    Consumable,
    Equipment,
    Material,
    Quest,
}

[CreateAssetMenu(fileName = "New Item", menuName = "Game/Item")]
public class ItemBase : ScriptableObject
{
    [Header("기본 정보")]
    public string itemName;
    [TextArea]
    public string description;
    public Sprite icon;
    public int ItemID;
    public float dropRate;

    [Header("분류")]
    public ItemType itemType;

    [Header("스택")]
    public bool isStackable = false;
    public int maxStack = 1;

    [Header("실행 시킬 코드")]
    public ItemUseAction useAction;
    //ItemUseAction스크립터블 오브젝트를 상속받는 코드를 작성해서 여기에 등록
    //ex) AddInventorySlots.cs
}
