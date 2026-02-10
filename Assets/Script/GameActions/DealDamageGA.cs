using System.Collections;
using System.Collections;using System.Collections.Generic;
using UnityEngine;

public class DealDamageGA : GameAction
{
    public int Amount { get; set; }
    public List<IDamageable> Targets { get; set; }
    public DealDamageGA(int amount, List<IDamageable> targets)
    { 
        Amount = amount;
        Targets = new List<IDamageable>(targets);
    }
}