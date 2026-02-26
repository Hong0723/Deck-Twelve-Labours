using UnityEngine;

public class EnemyStatus : MonoBehaviour
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

    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        if (hpBar != null) hpBar.Set(currentHP, maxHP, 0);

        if (currentHP <= 0)
        {
            if (handler != null) handler.DisplayVictory();
            gameObject.SetActive(false); // 적 파괴/비활성화
        }
    }
}