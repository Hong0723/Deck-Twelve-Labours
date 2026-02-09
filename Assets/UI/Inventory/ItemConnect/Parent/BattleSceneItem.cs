using UnityEngine;

public class BattleSceneItem : MonoBehaviour
{
    public virtual void SetStaticItemData(ItemBase itembase)
    {
        Debug.Log("Override 되지 않았습니다 상속후 Override해서 구체적인 내용을 작성해주세요");
    }
}
