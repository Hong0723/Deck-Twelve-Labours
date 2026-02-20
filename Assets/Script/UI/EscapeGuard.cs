using UnityEngine;

public static class EscapeGuard
{
    private static int escConsumedFrame = -1;

    public static void ConsumeEsc()
    {
        escConsumedFrame = Time.frameCount;
    }

    public static bool EscConsumedThisFrame()
    {
        return escConsumedFrame == Time.frameCount;
    }
}