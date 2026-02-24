using UnityEngine;

public class EnemyStatus : MonoBehaviour, IDamageable
{
    public HPBar hpBar;
    public GameOverHandler handler;
    public Player player; // 공격 대상인 플레이어를 연결
    public int maxHP = 100;
    private int currentHP;

    
    void Start()
    {
        maxHP= DeliverBattleData.MonsterInfo.maxHP;//스크립터블 오브젝트로 몬스터 체력 관리
        currentHP = maxHP;
        if (hpBar != null) hpBar.Set(currentHP, maxHP, 0);        
    }

    void Update()
    {
    }
    public void SetHP(int value)
    {
        currentHP = value;
    }
    public void TakeDamage(int damage)
    {
        //--지금 HP관리하는부분이 Enemy.cs랑 중복되어 관리하고 있어서
        //일단은 Enemy.cs부분에 힐, 쉴드, 디펜스 등 로직이 더 많아서 그쪽에서
        //관리하는 걸로 해놨습니다. 다만 밑에 currentHp가 0이하일때 발생하는 부분은
        //Enemy.cs부분에 없어서 currentHp가 0이하일때 발생하는 부분은 사용합니다.
        //더 좋은 생각 있으시면 Enemy.cs 로직을 감안해서 여기에 구현 하셔도 됩니다
        
        //--
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        Debug.Log("CurrentHP: " + currentHP);
        //if (hpBar != null) hpBar.Set(currentHP, maxHP, 0);Enemy.cs에서 관리

        //이부분은 사용합니다.
        if (currentHP <= 0)
        {
            GetComponent<Enemy>()?.OnDeathFromStatus();
        }
    } 
    
}