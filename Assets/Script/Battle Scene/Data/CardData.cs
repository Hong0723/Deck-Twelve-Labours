using NUnit.Framework;
using System.Collections.Generic;
using SerializeReferenceEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Card")]
public class CardData : ScriptableObject
{
    [field: SerializeField] public string Description {get; private set;}

    [field: SerializeField] public int Mana {get; private set;}

    [field: SerializeField] public Sprite Image {get; private set;}

    //카드사용시 플레이어한테 이펙트 추가 
    //[field: SerializeField] public CardEff Effect { get; private set; }

    [field: SerializeReference, SR] public Effect ManualTargetEffect { get; private set; }
    [field: SerializeReference, SR] public List<Effect> Effects { get; private set; }
}

