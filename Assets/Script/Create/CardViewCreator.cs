using DG.Tweening;
using UnityEngine;

public class CardViewCreator : Singleton<CardViewCreator>
{
    [SerializeField] private cardsView cardViewPrefab;
    
    public cardsView CreateCardView(Vector3 position, Quaternion rotation)
    {
        cardsView cardView = Instantiate(cardViewPrefab, position, rotation);
        cardView.transform.localScale = Vector3.zero;
        cardView.transform.DOScale(Vector3.one, 0.15f);
        return cardView;
    }
}
