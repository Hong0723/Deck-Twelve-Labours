using Unity.VisualScripting;
using UnityEngine;

public class AlarmEndEffect : MonoBehaviour
{

    [SerializeField]
    private GameObject enemyObj;    
    private MonsterSkill effectAccureScript;

    [SerializeField]
    private GameObject playerObj;
    private PlayerSkill playerEffectAccureScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerEffectAccureScript=playerObj.GetComponent<PlayerSkill>();
    }

    public void EndEffectAttack()
    {

        effectAccureScript.EndEffectAttack();
    }

    public void EndEffectAttackImpact()
    {

        effectAccureScript.EndEffectAttackImpact();
    }

    public void EndEffectShield()
    {
        
        effectAccureScript.EndEffectShield();
    }

    public void EndEffectHeal()
    {
        effectAccureScript.EndEffectHeal();
    }

    public void EndEffectDefense()
    {
        effectAccureScript.EndEffectDefense();
    }



    //플레이어용
    public void EndEffectAttackPlayer()
    {
        playerEffectAccureScript.EndEffectAttack();
    }

    public void EndEffectAttackImpactPlayer()
    {
        playerEffectAccureScript.EndEffectAttackImpact();
    }
}
