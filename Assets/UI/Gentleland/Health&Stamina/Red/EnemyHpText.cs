using TMPro;
using UnityEngine;

public class EnemyHpText : MonoBehaviour
{
    public TMP_Text MaxHP;
    public TMP_Text CurrentHP;
    public TMP_Text EnemyName;
    public Enemy EnemyScript;
    //public int debugHP;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MaxHP.text = DeliverBattleData.MonsterInfo.maxHP.ToString();
        //debugHP = DeliverBattleData.MonsterInfo.maxHP;
        CurrentHP.text = MaxHP.text;
        EnemyName.text = DeliverBattleData.MonsterInfo.monsterName;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentHP.text = EnemyScript.currentHP.ToString();
    }
}
