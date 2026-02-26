using UnityEngine;

public class HealGA : GameAction
{
    public int Amount { get; private set; }
    public Player Target { get; private set; }

    public HealGA( int  amount, Player targets )
    {
        Amount = amount;
        Target = targets;
    }
}
