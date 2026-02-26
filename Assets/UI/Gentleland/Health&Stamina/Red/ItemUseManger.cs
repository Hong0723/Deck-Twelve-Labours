using Unity.VisualScripting;
using UnityEngine;

public class ItemUseManager : HPBar
{
    public static ItemUseManager Instance;
    [SerializeField] private MonsterType PlayerInfo;

    void Awake()
    {        
        base.Awake();
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
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


    public static void SetPlayerShieldAmount(int val)
    {
        Instance.UpdateShieldStat(val);
    }


    void UpdateShieldStat(int val)
    {
        PlayerInfo.shield += val;
    }

    public static void SetPlayerHealAmount(int val)
    {
        Instance.UpdateHealStat(val);
    }


    void UpdateHealStat(int val)
    {
        PlayerInfo.Heal += val;
    }
}
