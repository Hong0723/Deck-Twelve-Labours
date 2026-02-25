using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class ActionSystem : Singleton<ActionSystem>
{
    private void OnEnable()
    {
        AttachPerformer<GainShieldGA>(GainShieldPerformer);
    }

    private readonly Stack<List<GameAction>> reactionTargets = new();
    [SerializeField] private bool isPerformingDebug;
    public bool IsPerforming 
    { 
        get => isPerformingDebug; 
        private set => isPerformingDebug = value; 
    }
    private static Dictionary<Type, List<Action<GameAction>>> preSubs = new();
    private static Dictionary<Type, List<Action<GameAction>>> postSubs = new();
    private static Dictionary<Type, Dictionary<Delegate, Action<GameAction>>> preSubWrappers = new();
    private static Dictionary<Type, Dictionary<Delegate, Action<GameAction>>> postSubWrappers = new();
    private static Dictionary<Type, Func<GameAction, IEnumerator>> performers = new();
    public void Perform(GameAction action, System.Action OnPerformFinished = null)
    {
        if (IsPerforming) return;
        IsPerforming = true;
        StartCoroutine(Flow(action, () =>
        {
            IsPerforming = false;
            OnPerformFinished?.Invoke();
        }));
    }
    public void AddReaction(GameAction gameAction)
    {
        if (gameAction == null) return;
        if (reactionTargets.Count == 0)
        {
            Debug.LogWarning("현재 반응을 추가할 대상 리스트가 없습니다.");
            return;
        }

        reactionTargets.Peek().Add(gameAction);
    }
    private IEnumerator Flow(GameAction action, Action OnFlowFinished = null)
    {
        if (action == null)
        {
            OnFlowFinished?.Invoke();
            yield break;
        }

        yield return RunPhase(action.PreReactions, subscribers: () => PerformSubscribers(action, preSubs));
        yield return RunPhase(action.PerformReactions, performer: () => PerformPerformer(action));
        yield return RunPhase(action.PostReactions, subscribers: () => PerformSubscribers(action, postSubs));

        OnFlowFinished?.Invoke();
    }
    private IEnumerator RunPhase(List<GameAction> phaseReactions, Func<IEnumerator> performer = null, Action subscribers = null)
    {
        reactionTargets.Push(phaseReactions);
        try
        {
            subscribers?.Invoke();
            if (performer != null)
            {
                yield return performer();
            }
            yield return PerformReactions(phaseReactions);
        }
        finally
        {
            reactionTargets.Pop();
        }
    }
    private IEnumerator PerformPerformer(GameAction action)
    {
        Type type = action.GetType();
        if (performers.ContainsKey(type))
        {
            yield return performers[type](action);
        }
    }
    private void PerformSubscribers(GameAction action, Dictionary<Type, List<Action<GameAction>>> subs)
    {
        Type type = action.GetType();
        if (subs.ContainsKey(type))
        {
            foreach (var sub in subs[type])
            {
                sub(action);
            }
        }
    }
    private IEnumerator PerformReactions(List<GameAction> reactions)
    {
        if (reactions == null) yield break;

        for (int i = 0; i < reactions.Count; i++)
        {
            var reaction = reactions[i];
            if (reaction == null)
            {
                Debug.LogWarning("null Reaction을 건너뜁니다.");
                continue;
            }
            yield return Flow(reaction);
        }
    }
    public static void AttachPerformer<T>(Func<T, IEnumerator> performer) where T : GameAction
    {
        Type type = typeof(T);
        IEnumerator wrappedPerformer(GameAction action) => performer((T)action);
        if (performers.ContainsKey(type)) performers[type] = wrappedPerformer;
        else performers.Add(type, wrappedPerformer);
    }
    public static void DetachPerformer<T>() where T : GameAction
    {
        Type type = typeof(T);
        if (performers.ContainsKey(type)) performers.Remove(type);
    }
    public static void SubscribeReaction<T>(Action<T> reaction, ReactionTiming timing) where T : GameAction
    {
        Dictionary<Type, List<Action<GameAction>>> subs = timing == ReactionTiming.PRE ? preSubs : postSubs;
        Dictionary<Type, Dictionary<Delegate, Action<GameAction>>> wrapperMapByType = timing == ReactionTiming.PRE ? preSubWrappers : postSubWrappers;
        Type type = typeof(T);

        if (!wrapperMapByType.ContainsKey(type))
        {
            wrapperMapByType[type] = new();
        }

        // 같은 메서드가 중복 등록되지 않도록 보호
        if (wrapperMapByType[type].ContainsKey(reaction))
        {
            return;
        }

        void wrappedReaction(GameAction action) => reaction((T)action);
        wrapperMapByType[type][reaction] = wrappedReaction;

        if (!subs.ContainsKey(type))
        {
            subs.Add(type, new());
        }
        subs[type].Add(wrappedReaction);
    }
    public static void UnsubscribeReaction<T>(Action<T> reaction, ReactionTiming timing) where T : GameAction
    {
        Dictionary<Type, List<Action<GameAction>>> subs = timing == ReactionTiming.PRE ? preSubs : postSubs;
        Dictionary<Type, Dictionary<Delegate, Action<GameAction>>> wrapperMapByType = timing == ReactionTiming.PRE ? preSubWrappers : postSubWrappers;
        Type type = typeof(T);

        if (!subs.ContainsKey(type) || !wrapperMapByType.ContainsKey(type))
        {
            return;
        }

        if (!wrapperMapByType[type].TryGetValue(reaction, out var wrappedReaction))
        {
            return;
        }

        subs[type].Remove(wrappedReaction);
        wrapperMapByType[type].Remove(reaction);
    }
    private IEnumerator GainShieldPerformer(GainShieldGA action)
    {
        if (action == null) yield break;
        if (action.Target == null) yield break;

        action.Target.GainShield(action.Amount);
        yield break;
    }
}
