using UnityEngine;

public class PlayerStatus : MonoBehaviour, IDamageable
{
    public HPBar hpBar;
    public GameOverHandler handler;
    public EnemyStatus enemy; // 공격 대상인 적을 연결 (테스트용)
    public int maxHP = 100;

    private void Start()
    {
        // 전역 HP 초기화(이미 초기화되어 있으면 유지)
        GlobalPlayerHP.InitializeIfNeeded(maxHP);
        GlobalPlayerHP.UpdateMaxHP(maxHP);

        RefreshHPBar();
    }

    private void Update()
    {
        // (원본 유지) 스페이스바를 누르면 적(EnemyStatus)의 피를 깎음
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (enemy != null)
            {
                enemy.TakeDamage(20);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        //--EnemyStatus.cs랑 마찬가지로 체력바를 중복해서 관리하고 있어서
        //로직이 더 많은 Player.cs에서 관리하게 일단은 해놨습니다.
        //밑의 if문은 Player.cs에는 없는부분이고 사용되어야 해서 
        //if (GlobalPlayerHP.IsDead()) 아래내용은 여기서 사용합니다.
        //더 좋은 생각 있으시면 Player.cs 로직을 감안해서 여기에 구현 하셔도 됩니다
        //GlobalPlayerHP.Damage(damage);
        //RefreshHPBar();

        if (GlobalPlayerHP.IsDead())
        {
            // 게임 오버 UI 표시(기존 동작 유지)
            if (handler != null) handler.DisplayGameOver();

            // 게임 오버 시 HP 초기화(정책)
            GlobalPlayerHP.ResetOnGameOver();
            RefreshHPBar();
        }
    }

    private void RefreshHPBar()
    {
        if (hpBar != null)
        {
            hpBar.Set(GlobalPlayerHP.CurrentHP, GlobalPlayerHP.MaxHP, 0);
        }
    }
}
