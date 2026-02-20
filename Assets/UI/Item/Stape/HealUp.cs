using UnityEngine;

[CreateAssetMenu(menuName = "Game/ItemUse/PlayerHealUP")]
public class HealUp : ItemUseAction
{
    [SerializeField] private int healAmount = 3; // ← 추가
    public override void Execute(GameObject user)
    {
        ItemUseManager.SetPlayerHealAmount(healAmount);
        Debug.Log("회복량 Up");
    }
}