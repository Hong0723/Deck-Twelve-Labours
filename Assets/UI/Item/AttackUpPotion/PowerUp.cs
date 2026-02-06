using UnityEngine;

[CreateAssetMenu(menuName = "Game/ItemUse/PlayerAttackPowerUP")]
public class PowerUP : ItemUseAction
{
    public override void Execute(GameObject user)
    {
        ItemUseManager.SetPlayerAttackDamage(4);
        Debug.Log("공격력 Up");
    }
}