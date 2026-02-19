using System.Collections.Generic;
using UnityEngine;

public static class GlobalItemState
{
    private static HashSet<string> PickedItems = new HashSet<string>();
    public static void MarkAsPicked(string itemName, int itemID)
    {
        string key = $"{itemName}_{itemID}";
        PickedItems.Add(key);
    }

    public static bool IsPicked(string itemName, int itemID)
    {
        string key = $"{itemName}_{itemID}";
        return PickedItems.Contains(key);
    }
    public static void ResetAll()
    {
        PickedItems.Clear();
    }
}
