using UnityEngine;

[CreateAssetMenu(menuName = "Game/ItemUse/PlayerAttackPowerUP")]
public class PowerUP : ItemUseAction
{
    [SerializeField] private int attackAmount = 4; // ← 추가
    public override void Execute()
    {
        ItemUseManager.SetPlayerAttackDamage(attackAmount);
        Debug.Log("공격력 Up");
    }
}