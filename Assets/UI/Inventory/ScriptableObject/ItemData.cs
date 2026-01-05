using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;//아이템 이름
    public int itemID;//0은 플레이어무기 1은 몬스터무기? 일단 추가해놈
    public Sprite itemIcon;//아이템 이미지
    public string description;//아이템 설명
}
