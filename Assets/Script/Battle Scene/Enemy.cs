using UnityEngine;
using System.Collections.Generic;

public enum EnemyActionType
{
    Attack,
    Shield,
    Heal,
    Defense
}

public class Enemy : MonoBehaviour
{
    public int maxHP = 40;
    public int currentHP;
    public int shield;

    public bool isDefending;

    public int defenseReduce = 40;
    public int counterDamage = 4;

    public EnemyActionType nextAction;
    public HPBar hpBar;

    void Start()
    {
        currentHP = maxHP;
        shield = 0;
        isDefending = false;

        DecideNextAction();
        UpdateHPBar();
    }

    // =========================
    // 턴 훅
    // =========================
    public void OnPlayerTurnStart()
    {
        if (nextAction == EnemyActionType.Defense)
        {
            isDefending = true;
            Debug.Log("Enemy 방어 태세");
        }
    }

    public void OnEnemyTurnStart()
    {
        shield = 0;
        isDefending = false;
        UpdateHPBar();
    }

    // =========================
    // Enemy 행동
    // =========================
    public void TakeTurn()
    {
        ExecuteAction();
        DecideNextAction();
    }

    void ExecuteAction()
    {
        switch (nextAction)
        {
            case EnemyActionType.Attack:
                BattleManager.Instance.player.TakeDamage(8);
                break;

            case EnemyActionType.Shield:
                shield += 6;
                break;

            case EnemyActionType.Heal:
                currentHP = Mathf.Min(maxHP, currentHP + 5);
                break;

            case EnemyActionType.Defense:
                // 실행 없음 (상태용)
                break;
        }

        UpdateHPBar();
    }

    // =========================
    // Player 공격 대응
    // =========================
    public void TakeHitFromPlayer(int damage)
    {
        int finalDamage = damage;

        if (isDefending)
        {
            finalDamage = Mathf.Max(damage - defenseReduce, 0);
            BattleManager.Instance.player.TakeDamage(counterDamage);
            Debug.Log("Defense 반격 발동");
        }

        int absorbed = Mathf.Min(shield, finalDamage);
        shield -= absorbed;
        finalDamage -= absorbed;

        currentHP = Mathf.Max(currentHP - finalDamage, 0);
        UpdateHPBar();
    }

    // =========================
    // 패턴
    // =========================
    void DecideNextAction()
    {
        List<EnemyActionType> pool = new()
        {
            EnemyActionType.Attack,
            EnemyActionType.Shield,
            EnemyActionType.Defense
        };

        if (currentHP < maxHP * 0.8f)
            pool.Add(EnemyActionType.Heal);

        nextAction = pool[Random.Range(0, pool.Count)];
    }

    void UpdateHPBar()
    {
        if (hpBar)
            hpBar.Set(currentHP, maxHP, shield);
    }
}
