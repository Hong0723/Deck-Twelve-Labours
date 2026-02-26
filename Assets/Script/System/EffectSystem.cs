using System.Collections;
using UnityEngine;

public class EffectSystem : MonoBehaviour
{
    private void OnEnable()
    {
        ActionSystem.AttachPerformer<PerformEffectGA>(PerformEffectPerformer);
    }
    void OnDisable()
    {
        ActionSystem.DetachPerformer<PerformEffectGA>();
    }
    private IEnumerator PerformEffectPerformer(PerformEffectGA performEffectGA)
    {
        if (performEffectGA == null || performEffectGA.Effect == null)
        {
            Debug.LogWarning("PerformEffectGA 또는 Effect가 null 입니다. 효과 실행을 건너뜁니다.");
            yield break;
        }

        GameAction effectAction = performEffectGA.Effect.GetGameAction();
        if (effectAction == null)
        {
            Debug.LogWarning($"효과 액션 생성 실패: {performEffectGA.Effect.GetType().Name}");
            yield break;
        }

        ActionSystem.Instance.AddReaction(effectAction);
        yield return null;
    }
}
