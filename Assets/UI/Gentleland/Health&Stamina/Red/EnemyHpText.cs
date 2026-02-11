using TMPro;
using UnityEngine;

public class EnemyHpText : MonoBehaviour
{
    public TMP_Text MaxHP;
    public TMP_Text CurrentHP;
    public Enemy EnemyScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MaxHP.text = DeliverBattleData.MonsterInfo.maxHP.ToString();
        CurrentHP.text = MaxHP.text;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentHP.text = EnemyScript.currentHP.ToString();
    }
}
