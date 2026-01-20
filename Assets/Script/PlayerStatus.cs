using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public HPBar hpBar;
    public GameOverHandler handler; 
    public EnemyStatus enemy; // 공격 대상인 적을 연결
    public int maxHP = 100;
    private int currentHP;

    void Start()
    {
        currentHP = maxHP;
        if (hpBar != null) hpBar.Set(currentHP, maxHP, 0);
    }

    void Update()
    {
        // 스페이스바를 누르면 적(EnemyStatus)의 피를 깎음
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
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        if (hpBar != null) hpBar.Set(currentHP, maxHP, 0);

        if (currentHP <= 0)
        {
            if (handler != null) handler.DisplayGameOver();
        }
    }
}