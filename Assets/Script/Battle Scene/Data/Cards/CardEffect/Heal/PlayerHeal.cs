using UnityEngine;

[CreateAssetMenu(fileName = "CardEff", menuName = "Scriptable Objects/CardEff/PlayerHeal")]
public class PlayerHeal : CardEff
{
    public override void ExecuteEff()
    {
        PlayerSkill.EffHeal();
        Debug.Log("heal¿Ã∆Â∆Æ");
    }
}
