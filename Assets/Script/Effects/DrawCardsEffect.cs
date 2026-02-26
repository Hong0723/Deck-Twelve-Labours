using UnityEngine;

public class DrawCardsEffect : Effect
{
    [Min(1)]
    [SerializeField] private int drawAmount;
    public override GameAction GetGameAction()
    {
        DrawCardsGA drawCardsGA = new(drawAmount);
        return drawCardsGA;
    }
}

