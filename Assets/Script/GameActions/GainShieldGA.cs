using System.Collections.Generic;
using UnityEngine;

public class GainShieldGA : GameAction
{
    public int Amount { get; private set; }
    public Player Target { get; private set; }

    public GainShieldGA(int amount, Player targets)
    {
        Amount = amount;
        Target = targets;
    }

}
