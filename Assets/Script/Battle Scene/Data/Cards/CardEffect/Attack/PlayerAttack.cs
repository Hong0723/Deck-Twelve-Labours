using UnityEngine;

[CreateAssetMenu(fileName = "CardEff", menuName = "Scriptable Objects/CardEff/PlayerAttack")]
public class PlayerAttack : CardEff
{
    public override void ExecuteEff()
    {
        PlayerSkill.EffAttack();
        Debug.Log("공격이펙트");        
    }
}
