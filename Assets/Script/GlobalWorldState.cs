using UnityEngine;

public static class GlobalWorldState
{
    public static Vector3 lastPlayerPosition;
    public static bool hasSavedPosition;
    
    public static void ResetAll()
    {
        lastPlayerPosition = Vector3.zero;
        hasSavedPosition = false;
    }
}