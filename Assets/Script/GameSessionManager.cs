using UnityEngine;

public static class GameSessionManager
{
    public static void ResetAll()
    {
        GlobalMonsterState.ResetAll();
        GlobalPlayerHP.ResetAll();

        GlobalWorldState.hasSavedPosition = false;
        GlobalWorldState.lastPlayerPosition = Vector3.zero;
    }
}