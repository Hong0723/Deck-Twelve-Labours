using TMPro;
using UnityEngine;

public class PlayerHPText : MonoBehaviour
{    
    public TMP_Text MaxHP;
    public TMP_Text CurrentHP;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MaxHP.text = DeliverBattleData.PlayerInfo.maxHP.ToString();
        CurrentHP.text = GlobalPlayerHP.CurrentHP.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        CurrentHP.text = GlobalPlayerHP.CurrentHP.ToString();
    }
}
