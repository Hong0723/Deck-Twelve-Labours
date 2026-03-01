using UnityEngine;

public class CombatContext : MonoBehaviour
{
    public static CombatContext Instance { get; private set; }

    public Sprite selectedBattleBackground;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetBackground(Sprite s) => selectedBattleBackground = s;
    public void Clear() => selectedBattleBackground = null;
}