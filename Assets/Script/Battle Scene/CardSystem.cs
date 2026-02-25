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
        if (deckData == null)
        {
            Debug.LogError("CardSystem.Setup 실패: deckData가 null 입니다.");
            return;
        }

        int validCount = 0;
        for (int i = 0; i < deckData.Count; i++)
        {
            if (deckData[i] != null) validCount++;
        }

        if (validCount == 0)
        {
            Debug.LogWarning("CardSystem.Setup 호출됨: 유효한 카드가 0장이라 기존 덱을 유지합니다.");
            return;
        }

        drawPile.Clear();
        discardPile.Clear();
        hand.Clear();

        for (int i = 0; i < deckData.Count; i++)
        {
            CardData cardData = deckData[i];
            if (cardData == null)
            {
                Debug.LogWarning($"덱 데이터 {i}번 슬롯이 비어 있습니다(null). 건너뜁니다.");
                continue;
            }

            Card card = new(cardData);
            drawPile.Add(card);
        }

        Debug.Log($"CardSystem.Setup 완료: 유효 카드 {drawPile.Count}장");
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
        if (playCardGA.Card.Effects == null) yield break;

        foreach (var effect in playCardGA.Card.Effects)
        {
            if (effect == null)
            {
                Debug.LogWarning($"카드 '{playCardGA.Card.Title}' 에 null Effect가 있습니다. 해당 효과는 건너뜁니다.");
                continue;
            }

            // 수동 타겟 공격카드가 일반 효과 리스트에도 같은 공격 효과를 갖고 있으면 데미지가 2번 들어간다.
            if (playCardGA.Card.ManualTargetEffect is AttackCardsEffect && effect is AttackCardsEffect)
            {
                Debug.LogWarning($"카드 '{playCardGA.Card.Title}' 에 공격 효과가 중복 설정되어 있어 일반 효과의 공격을 건너뜁니다. (ManualTargetEffect + Effects)");
                continue;
            }

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
