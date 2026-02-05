using Unity.VisualScripting;
using UnityEngine;

public class ItemUseManager : HPBar
{
    public static ItemUseManager Instance;
    [SerializeField] private MonsterType PlayerInfo;

    void Awake()
    {
        Instance = this;
        base.Awake();

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateGameSceneBar();
    }

    public static void UpdateGameScenePlayerHp(int val)
    {
        GlobalPlayerHP.SetCurrentHP(val);
        Instance.UpdateGameSceneBar();
    }

    void UpdateGameSceneBar()
    {
        base.Set(GlobalPlayerHP.CurrentHP, GlobalPlayerHP.MaxHP, 0);
    }

    public static void SetPlayerAttackDamage(int val)
    {
        Instance.UpdateAttackStat(val);
    }

    void UpdateAttackStat(int val)
    {
        //Áõ°¡°ª
        PlayerInfo.AttackDamage += val;
    }
}
