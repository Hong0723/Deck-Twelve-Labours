using UnityEngine;

[CreateAssetMenu(menuName = "Game/ItemUse/GameScenePlayerHPUP")]
public class GameSceneHealthUP : ItemUseAction
{
    public override void Execute(GameObject user)
    {
        ItemUseManager.UpdateGameScenePlayerHp(GlobalPlayerHP.CurrentHP + 10);
        Debug.Log("체력회복");
        Debug.Log(
        "현재체력 " + GlobalPlayerHP.CurrentHP +
        " 최대체력 " + GlobalPlayerHP.MaxHP
        );        
    }
}
