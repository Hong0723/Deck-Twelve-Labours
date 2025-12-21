using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHP = 30;
    public int currentHP;
    public int block;

    public HPBar hpBar;

    void Start()
    {
        currentHP = maxHP;
        block = 0;
        UpdateHPBar();
    }

    public void TakeDamage(int dmg)
    {
        currentHP = Mathf.Max(currentHP - dmg, 0);
        UpdateHPBar();
    }

    public void Heal(int amount)
    {
        currentHP = Mathf.Min(maxHP, currentHP + amount);
        UpdateHPBar();
    }

    void UpdateHPBar()
    {
        if (hpBar)
            hpBar.Set(currentHP, maxHP, 0);
    }

    public bool IsDead() => currentHP <= 0;
}
