using Unity.VisualScripting;
using UnityEngine;

public class ItemUseManager : HPBar
{
    public static ItemUseManager Instance;
    [SerializeField] private MonsterType PlayerInfo;

    void Awake()
    {
        //Instance = this;
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
        GlobalPlayerHP.SetCurrentHP(GlobalPlayerHP.MaxHP);//플레이어 체력 초기화
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
        //증가값
        PlayerInfo.AttackDamage += val;
    }
}
