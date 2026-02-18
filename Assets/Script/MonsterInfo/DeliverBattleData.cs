using System;
using UnityEngine;

public static class DeliverBattleData
{
    public static MonsterType MonsterInfo;
    public static MonsterType PlayerInfo;
    public static ItemBase[] BattleSceneItems = new ItemBase[3];

    internal static void Clear()
    {
    MonsterInfo = null;
    PlayerInfo = null;

    for (int i = 0; i < BattleSceneItems.Length; i++)
        BattleSceneItems[i] = null;

    }
}
