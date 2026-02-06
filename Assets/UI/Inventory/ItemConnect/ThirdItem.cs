using UnityEngine;

public class ThirdItem : BattleSceneItem
{
    // 오버라이드
    public override void SetStaticItemData(ItemBase itembase)
    {
        DeliverBattleData.BattleSceneItems[2] = itembase;
        Debug.Log("세번째");
    }
}
