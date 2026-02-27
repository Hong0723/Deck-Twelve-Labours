using UnityEngine;

public class CombatContext : MonoBehaviour
{
    public static CombatContext Instance { get; private set; }

    [Tooltip("Battle Scene에서 사용할 배경 스프라이트")]
    public Sprite selectedBattleBackground;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetBackground(Sprite sprite) => selectedBattleBackground = sprite;
    public void Clear() => selectedBattleBackground = null;
}