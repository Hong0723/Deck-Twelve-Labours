using UnityEngine;

public class HealCardsEffect : Effect
{
    [Min(1)]
    [SerializeField] private int healAmount;

    public override GameAction GetGameAction()
    {
        Debug.Log($"[HealCardsEffects] called. healAmount={healAmount}");

        Player player = Object.FindFirstObjectByType<Player>();
        if ( player == null ) return null;

        return new HealGA(healAmount, player);
    }
}
