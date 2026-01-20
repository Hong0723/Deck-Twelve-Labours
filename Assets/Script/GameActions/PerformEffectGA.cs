using UnityEngine;

public class PerformEffectGA : GameAction
{
    public Effect Effect { get; private set; }
    public PerformEffectGA(Effect effect)
    {
        Effect = effect;
    }
}// CardSystem 이어서 수정해야 함
