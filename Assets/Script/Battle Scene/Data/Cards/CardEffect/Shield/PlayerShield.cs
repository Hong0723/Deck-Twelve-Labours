using UnityEngine;

[CreateAssetMenu(fileName = "CardEff", menuName = "Scriptable Objects/CardEff/PlayerShield")]
public class PlayerShield : CardEff
{
    public override void ExecuteEff()
    {
        PlayerSkill.EffShield();
        Debug.Log("shield¿Ã∆Â∆Æ");
    }
}
