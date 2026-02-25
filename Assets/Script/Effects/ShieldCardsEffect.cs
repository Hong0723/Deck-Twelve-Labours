using System.Collections.Generic;
using UnityEngine;

public class ShieldCardsEffect : Effect
{
    [Min(1)]
    [SerializeField] private int shieldAmount;

    public override GameAction GetGameAction()
    {
        Player player = Object.FindFirstObjectByType<Player>();
        if (player == null) return null;
        return new GainShieldGA(shieldAmount, player);
    }
}
