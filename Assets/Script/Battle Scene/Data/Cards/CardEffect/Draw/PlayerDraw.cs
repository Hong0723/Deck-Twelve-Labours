using UnityEngine;

[CreateAssetMenu(fileName = "CardEff", menuName = "Scriptable Objects/CardEff/PlayerDraw")]

public class PlayerDraw : CardEff
{
    public override void ExecuteEff()
    {
        PlayerSkill.EffDraw();
        Debug.Log("드로우 이펙트");
    }
}
