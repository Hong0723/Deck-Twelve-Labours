using UnityEngine;

public class EnemyStatus : MonoBehaviour, IDamageable
{
    public HPBar hpBar;
    public GameOverHandler handler;
    public PlayerStatus player; // 공격 대상인 플레이어를 연결
    public int maxHP = 100;
    private int currentHP;

    
    void Start()
    {
        currentHP = maxHP;
        if (hpBar != null) hpBar.Set(currentHP, maxHP, 0);        
    }

    void Update()
    {
        // [주의] 적 스크립트에서는 절대로 KeyCode.Space를 사용하지 마세요!
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (player != null)
            {
                player.TakeDamage(20);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        //--지금 HP관리하는부분이 Enemy.cs랑 중복되어 관리하고 있어서
        //일단은 Enemy.cs부분에 힐, 쉴드, 디펜스 등 로직이 더 많아서 그쪽에서
        //관리하는 걸로 해놨습니다. 다만 밑에 currentHp가 0이하일때 발생하는 부분은
        //Enemy.cs부분에 없어서 currentHp가 0이하일때 발생하는 부분은 사용합니다.
        //더 좋은 생각 있으시면 Enemy.cs 로직을 감안해서 여기에 구현 하셔도 됩니다
        Enemy enemy = GetComponent<Enemy>();
        enemy.TakeHitFromPlayer(damage);

        //--
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        //if (hpBar != null) hpBar.Set(currentHP, maxHP, 0);Enemy.cs에서 관리

        //이부분은 사용합니다.
        if (currentHP <= 0)
        {
            if (handler != null) handler.DisplayVictory();
            gameObject.SetActive(false); // 적 파괴/비활성화
        }
    } 
    
}