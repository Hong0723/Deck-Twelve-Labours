using UnityEngine;

[CreateAssetMenu(menuName = "Game/ItemUse/Add Inventory Slot")]
public class AddInventorySlots : ItemUseAction
{
    public override void Execute(GameObject user)
    {
        Inventory.Instance.AddInventorySlot(4);
        Debug.Log("ΩΩ∑‘»Æ¿Â");
    }
}
