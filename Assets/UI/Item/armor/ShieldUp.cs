using UnityEngine;

[CreateAssetMenu(menuName = "Game/ItemUse/PlayerShieldUp")]
public class ShieldUp : ItemUseAction
{
    [SerializeField] private int ShieldAmount = 2; // ¡ç Ãß°¡

    public override void Execute()
    {
        ItemUseManager.SetPlayerShieldAmount(ShieldAmount);
        Debug.Log("½¯µå Up");
    }
}
