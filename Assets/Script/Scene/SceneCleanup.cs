using UnityEngine;
using DG.Tweening;

public class SceneCleanup : MonoBehaviour
{
    void Awake()
    {
        Time.timeScale = 1f;
        var fade = GameObject.Find("PortalFadeCanvas");
        if(fade != null)
        {
            fade.SetActive(false);
        }
        DOTween.KillAll();   // 씬 시작 시 잔여 Tween 정리
    }
}