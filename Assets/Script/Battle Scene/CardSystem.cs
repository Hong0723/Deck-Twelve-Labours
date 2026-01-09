using System.Collections.Generic;
using UnityEngine;

public class CardSystem : MonoBehaviour
{
    [SerializeField] private List<CardData> initialDeck = new();

    [Header("Runtime piles")]
    public List<CardData> drawPile = new();     
    public List<CardData> hand = new();         
    public List<CardData> discardPile = new();  

    public void Init()
    {
        drawPile.Clear();
        hand.Clear();
        discardPile.Clear();

        drawPile.AddRange(initialDeck);
        Shuffle(drawPile);
    }

    public void Draw(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (drawPile.Count == 0)
            {
                if (discardPile.Count == 0) return;
                drawPile.AddRange(discardPile);
                discardPile.Clear();
                Shuffle(drawPile);
            }

            CardData top = drawPile[drawPile.Count - 1];
            drawPile.RemoveAt(drawPile.Count - 1);
            hand.Add(top);
        }
    }

    public void DiscardFromHand(CardData card)
    {
        if (hand.Remove(card))
            discardPile.Add(card);
    }

    private void Shuffle(List<CardData> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int r = Random.Range(i, list.Count);
            (list[i], list[r]) = (list[r], list[i]);
        }
    }
}
