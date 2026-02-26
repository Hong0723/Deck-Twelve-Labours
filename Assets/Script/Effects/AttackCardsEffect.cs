using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackCardsEffect : Effect
{
    [Min(1)]
    [SerializeField] private int damageAmount;
    public override GameAction GetGameAction()
    {
        List<IDamageable> targets = new List<IDamageable>();
        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)

        if (enemy != null && enemy.currentHP > 0)
        {
            targets.Add(enemy);
            break;
        }
        
        return new DealDamageGA(damageAmount, targets);
    }
}
