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

    [Header("분류")]
    public ItemType itemType;

    [Header("스택")]
    public bool isStackable = false;
    public int maxStack = 1;
}
