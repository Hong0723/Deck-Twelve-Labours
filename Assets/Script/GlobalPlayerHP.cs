using UnityEngine;

/// <summary>
/// 전역(게임 전반)에서 동일한 HP 변수를 공유하기 위한 정적 상태 컨테이너.
/// - 씬이 바뀌어도 동일한 값을 사용합니다(Static).
/// - 게임 오버(HP<=0) 시 ResetOnGameOver()로 초기화할 수 있습니다.
/// </summary>
public static class GlobalPlayerHP
{
    public static bool IsInitialized { get; private set; }

    public static int MaxHP { get; private set; } = 1;
    public static int CurrentHP { get; private set; } = 1;

    /// <summary>
    /// 최초 1회 초기화. 이미 초기화되어 있으면 아무 것도 하지 않습니다.
    /// </summary>
    public static void InitializeIfNeeded(int maxHp)
    {
        if (IsInitialized) return;

        MaxHP = Mathf.Max(1, maxHp);
        CurrentHP = MaxHP;
        IsInitialized = true;
    }

    /// <summary>
    /// MaxHP를 최신값으로 맞춥니다(현재 HP는 clamp만 수행).
    /// 업그레이드/스탯 변경 등으로 MaxHP가 바뀔 수 있을 때 사용하세요.
    /// </summary>
    public static void UpdateMaxHP(int newMaxHp)
    {
        if (!IsInitialized)
        {
            InitializeIfNeeded(newMaxHp);
            return;
        }

        MaxHP = Mathf.Max(1, newMaxHp);
        CurrentHP = Mathf.Clamp(CurrentHP, 0, MaxHP);
    }

    public static void SetCurrentHP(int hp)
    {
        if (!IsInitialized) InitializeIfNeeded(MaxHP);
        CurrentHP = Mathf.Clamp(hp, 0, MaxHP);
    }

    public static void Damage(int damage)
    {
        if (damage <= 0) return;
        SetCurrentHP(CurrentHP - damage);
    }

    public static void Heal(int amount)
    {
        if (amount <= 0) return;
        SetCurrentHP(CurrentHP + amount);
    }

    public static bool IsDead()
    {
        return IsInitialized && CurrentHP <= 0;
    }

    /// <summary>
    /// 게임 오버 시 초기화 정책:
    /// - MaxHP는 유지
    /// - CurrentHP는 MaxHP로 복구
    /// </summary>
    public static void ResetOnGameOver()
    {
        if (!IsInitialized) return;
        CurrentHP = MaxHP;
    }
}
