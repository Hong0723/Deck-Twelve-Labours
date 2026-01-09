using UnityEngine;

public enum CardEffectType
{
    DamageEnemy,
    HealPlayer,
    GainBlock
}

[CreateAssetMenu(menuName = "Cards/CardData")]
public class CardData : ScriptableObject     
{
    public string cardName;        
    [TextArea] public string description; 
    public Sprite artwork;          

    public int cost;                
    public CardEffectType effectType;
    public int value;                
}
