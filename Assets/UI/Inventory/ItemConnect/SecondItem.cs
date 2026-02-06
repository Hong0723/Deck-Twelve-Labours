using UnityEngine;

public class SecondItem : BattleSceneItem
{
    // 오버라이드
    public override void SetStaticItemData(ItemBase itembase)
    {
        DeliverBattleData.BattleSceneItems[1] = itembase;
        Debug.Log("두번째");
    }
}
