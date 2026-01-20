using NUnit.Framework;
using System.Collections.Generic;
using System.Data.Common;
using UnityEditor;
using UnityEngine;

public class Card
{
    public string Title => data2.name;

    public string Description => data2.Description;

    public Sprite Image => data2.Image;

    public List<Effect> Effects => data2.Effects;

    public int Mana { get; private set; }
    private readonly CardData2 data2;

    public Card(CardData2 cardData2)
    {
        data2 = cardData2;
        Mana = cardData2.Mana;
    }
}