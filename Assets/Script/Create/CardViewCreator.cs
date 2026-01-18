using DG.Tweening;
using UnityEngine;

public class CardViewCreator : Singleton<CardViewCreator>
{
    [SerializeField] private cardsView cardViewPrefab;
    
    public cardsView CreateCardView(Card card, Vector3 position, Quaternion rotation)
    {
        cardsView cardView = Instantiate(cardViewPrefab, position, rotation);
        cardView.transform.localScale = Vector3.zero;
        cardView.transform.DOScale(new Vector3(10f, 10f, 1f), 0.15f);
        cardView.Setup(card);
        return cardView;
    }
}
