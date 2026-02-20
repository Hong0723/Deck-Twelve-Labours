using UnityEngine;

public class PerformEffectGA : GameAction
{
    public Effect Effect { get; private set; }

    public EffectContext Context { get; private set; } // 타겟 정보를 담을 컨텍스트 추가
    public PerformEffectGA(Effect effect)
    {
        Effect = effect;
    }
    public PerformEffectGA(Effect effect, EffectContext context)
    {
        Effect = effect;
        Context = context;
    }
}
