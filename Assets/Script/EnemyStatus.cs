using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public HPBar hpBar;
    public GameOverHandler gameOverHandler; // 추가: 직접 드래그해서 연결할 칸
    public int maxHP = 100;
    private int currentHP;

    void Start()
    {
        currentHP = maxHP;
        if (hpBar != null) hpBar.Set(currentHP, maxHP, 0);
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (hpBar != null) hpBar.Set(currentHP, maxHP, 0);

        if (currentHP <= 0) Die();
    }

    void Die()
    {
        // 직접 연결된 핸들러에게 승리창을 띄우라고 명령
        if (gameOverHandler != null)
        {
            gameOverHandler.DisplayVictory();
        }
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) TakeDamage(20);
    }
}