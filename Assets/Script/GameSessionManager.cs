using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameSessionManager
{

        public static void ResetGameSession()
    {
        // 1. 타임 초기화
        Time.timeScale = 1f;

        // 2. DOTween 전체 종료
        DOTween.KillAll();

        // 3. Static 데이터 초기화
        GlobalPlayerHP.ResetAll();
        GlobalMonsterState.ResetAll();
        GlobalWorldState.ResetAll();
        DeliverBattleData.Clear();

        // 4. DontDestroyOnLoad 오브젝트 제거
        ClearDontDestroyObjects();

        // 5. Start Scene 재로드
        SceneManager.LoadScene("Start Scene");
    }

    static void ClearDontDestroyObjects()
    {
        var objects = Object.FindObjectsOfType<GameObject>();

        foreach (var obj in objects)
        {
            if (obj.scene.name == "DontDestroyOnLoad")
            {
                Object.Destroy(obj);
            }
        }
    }  
}