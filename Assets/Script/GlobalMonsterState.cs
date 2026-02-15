using UnityEngine;
using System.Collections.Generic;

public static class GlobalMonsterState
{
    private static HashSet<string> defeatedMonsters = new HashSet<string>();

    public static void MarkAsDefeated(string monsterName)
    {
        defeatedMonsters.Add(monsterName);
    }

    public static bool IsDefeated(string monsterName)
    {
        return defeatedMonsters.Contains(monsterName);
    }
    public static void ResetAll()
{
    defeatedMonsters.Clear();
}
}