using UnityEngine;

public class PlayCardGA : GameAction
{
    public EnemyStatus ManualTarget { get; private set; }
    public Card Card { get; set; }
    public PlayCardGA(Card card)
    { 
        Card = card;
    }
    public PlayCardGA(Card card, EnemyStatus manualTarget)
    {
        Card = card;
        ManualTarget = manualTarget;
    }
}
