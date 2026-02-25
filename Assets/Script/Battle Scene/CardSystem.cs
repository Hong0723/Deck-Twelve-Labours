using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CardSystem : Singleton<CardSystem>
{
    [SerializeField] private HandView handView;
    [SerializeField] private Transform drawPilePoint;
    [SerializeField] private Transform discardPilePoint;

    private readonly List<Card> drawPile = new();
    private readonly List<Card> discardPile = new();
    private readonly List<Card> hand = new();

    [Header("Card SFX")]
    [SerializeField] private AudioClip drawCardSFX; 
    [SerializeField] private float drawCardVolume = 0.6f;
    [SerializeField] private AudioClip discardCardSFX;  
    [SerializeField] private float discardCardVolume = 0.55f;
    [SerializeField] private AudioClip reshuffleSFX;    
    [SerializeField] private float reshuffleVolume = 0.7f;

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<DrawCardsGA>(DrawCardsPerformer);
        ActionSystem.AttachPerformer<DiscardAllCardsGA>(DiscardAllCardsPerformer);
        ActionSystem.AttachPerformer<PlayCardGA>(PlayCardPerformer);
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPreReaction, ReactionTiming.PRE);
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<DrawCardsGA>();
        ActionSystem.DetachPerformer<DiscardAllCardsGA>();
        ActionSystem.DetachPerformer<PlayCardGA>();
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPreReaction, ReactionTiming.PRE);
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
    }

    public void Setup(List<CardData> deckData)
    {
        foreach(var cardData in deckData)
        {
            Card card = new(cardData);
            drawPile.Add(card);
        }
    }

    private IEnumerator DrawCardsPerformer(DrawCardsGA drawCardsGA)
    {
        Debug.Log($"뽑기 시작! 목표 장수: {drawCardsGA.Amount}");
        for (int i = 0; i < drawCardsGA.Amount; i++)
        {
            // 1. 만약 뽑을 카드가 없다면 덱을 다시 채움
            if (drawPile.Count == 0)
            {
                RefillDeck();
            }

            // 2. 덱을 채웠는데도 카드가 없다면 (전체 카드가 다 손에 있거나 아예 없는 경우) 
            // 더 이상 루프를 돌지 않고 종료
            if (drawPile.Count == 0)
            {
                Debug.LogWarning("더 이상 뽑을 카드가 없습니다.");
                yield break; // 
            }

            // 3. 카드가 있다면 한 장 뽑습니다.
            yield return DrawCard();
        }
    }

    private IEnumerator DiscardAllCardsPerformer(DiscardAllCardsGA discardAllCardsGA)
    {
        foreach (var card in hand)
        {
            discardPile.Add(card);
            CardView cardView = handView.RemoveCard(card);
            yield return DiscardCard(cardView);
        }
        hand.Clear();
    }

    private IEnumerator PlayCardPerformer(PlayCardGA playCardGA)
    {
        hand.Remove(playCardGA.Card);
        discardPile.Add(playCardGA.Card);

        CardView cardView = handView.RemoveCard(playCardGA.Card);
        yield return DiscardCard(cardView);
  
        SpendManaGA spendManaGA = new(playCardGA.Card.Mana);
        ActionSystem.Instance.AddReaction(spendManaGA);
        if (playCardGA.Card.ManualTargetEffect != null)
        {
            PerformEffectGA performEffectGA = new(playCardGA.Card.ManualTargetEffect, new() { Target = playCardGA.ManualTarget });
            ActionSystem.Instance.AddReaction(performEffectGA);
        }
        foreach (var effect in playCardGA.Card.Effects)
        {
            PerformEffectGA performEffectGA = new(effect);
            ActionSystem.Instance.AddReaction(performEffectGA);
        }
    }

    private void EnemyTurnPreReaction(EnemyTurnGA enemyTurnGA)
    {
        DiscardAllCardsGA discardAllCardsGA = new();
        ActionSystem.Instance.AddReaction(discardAllCardsGA);
    }

    private void EnemyTurnPostReaction(EnemyTurnGA enemyTurnGA)
    {
        DrawCardsGA drawCardsGA = new(5);
        ActionSystem.Instance.AddReaction(drawCardsGA);
    }

    private IEnumerator DrawCard()
    {
        Card card = drawPile.Draw();
        hand.Add(card);
        if (SFXManager.Instance != null)
        SFXManager.Instance.PlaySFX(drawCardSFX, drawCardVolume);
        CardView cardView = CardViewCreator.Instance.CreateCardView(card, drawPilePoint.position, drawPilePoint.rotation);
        yield return handView.AddCard(cardView);
    }

    private void RefillDeck()
    {
        
        if (discardPile.Count > 0)
    {
        // 🔊 셔플 사운드
        if (SFXManager.Instance != null)
            SFXManager.Instance.PlaySFX(reshuffleSFX, reshuffleVolume);
    }
        drawPile.AddRange(discardPile);
        discardPile.Clear();

        // 셔플 로직 추가 (List 확장 메서드나 Random 사용)
        for (int i = 0; i < drawPile.Count; i++)
        {
            Card temp = drawPile[i];
            int randomIndex = Random.Range(i, drawPile.Count);
            drawPile[i] = drawPile[randomIndex];
            drawPile[randomIndex] = temp;
        }
    }

    private IEnumerator DiscardCard(CardView cardView)
    {
        if (SFXManager.Instance != null)
        SFXManager.Instance.PlaySFX(discardCardSFX, discardCardVolume);
        cardView.transform.DOScale(Vector3.zero, 0.15f);
        Tween tween = cardView.transform.DOMove(discardPilePoint.position, 0.15f);
        yield return tween.WaitForCompletion();
        Destroy(cardView.gameObject);
    }
}
