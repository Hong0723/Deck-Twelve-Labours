using System.Data.Common;
using UnityEngine;

public class Card
{
    public string Title => data2.name;

    public string Description => data2.Description;

    public Sprite Image => data2.Image;

    public int Mana {get; private set;}
    private readonly CardData2 data2;

    public Card(CardData2 cardData2)
    {
        data2 = cardData2;
        Mana = cardData2.Mana;
    }
}
