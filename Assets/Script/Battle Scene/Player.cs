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
        int absorbed = Mathf.Min(block, dmg);
        block -= absorbed; 
        dmg -= absorbed; 

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
            hpBar.Set(currentHP, maxHP, block);
    }

    public bool IsDead() => currentHP <= 0;
}
