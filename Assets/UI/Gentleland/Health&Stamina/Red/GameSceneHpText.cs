using TMPro;
using UnityEngine;

public class GameSceneHpText : MonoBehaviour
{
    public TMP_Text MaxHP;
    public TMP_Text CurrentHP;
    public MonsterType PlayerInfo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        GlobalPlayerHP.InitializeIfNeeded(PlayerInfo.maxHP);
        MaxHP.text = PlayerInfo.maxHP.ToString();
        CurrentHP.text = GlobalPlayerHP.CurrentHP.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        CurrentHP.text = GlobalPlayerHP.CurrentHP.ToString();
    }
}
