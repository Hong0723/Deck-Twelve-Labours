using UnityEngine;

public class FirstItem : BattleSceneItem
{
    // 오버라이드
    public override void SetStaticItemData(ItemBase itembase)
    {
        DeliverBattleData.BattleSceneItems[0] = itembase;
        Debug.Log("첫번째");

    }
}
