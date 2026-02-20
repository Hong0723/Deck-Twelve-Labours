using NUnit.Framework;
using System.Collections.Generic;
using System.Data.Common;
using UnityEditor;
using UnityEngine;

public class Card
{
    public string Title => data.name;

    public string Description => data.Description;

    public Sprite Image => data.Image;

    public List<Effect> Effects => data.Effects;

    public int Mana { get; private set; }
    private readonly CardData data;

    // 데이터 원본(data)에서 값을 읽어오도록 연결
    public Effect ManualTargetEffect => data.ManualTargetEffect;    
    public Card(CardData cardData)
    {
        data = cardData;
        Mana = cardData.Mana;
    }
}