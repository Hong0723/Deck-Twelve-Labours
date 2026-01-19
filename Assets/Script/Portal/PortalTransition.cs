using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTransition : MonoBehaviour
{
    public static PortalTransition Instance { get; private set; }
    private bool _isTransitioning;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartTransition(Transform player, string targetSceneName, string targetSpawnId, float fadeOutTime, float fadeInTime)
    {
        if (_isTransitioning) return;
        StartCoroutine(CoTransition(player, targetSceneName, targetSpawnId, fadeOutTime, fadeInTime));
    }

    private IEnumerator CoTransition(Transform player, string targetSceneName, string targetSpawnId, float fadeOutTime, float fadeInTime)
    {
        _isTransitioning = true;

        // 1) 입력 잠금
        var lock1 = player != null ? player.GetComponent<IInputLock>() : null;
        lock1?.SetLocked(true);

        // 2) Fade Out
        if (FadeController.Instance != null)
            yield return FadeController.Instance.FadeTo(1f, fadeOutTime);

        // 3) 씬 로드
        yield return SceneManager.LoadSceneAsync(targetSceneName, LoadSceneMode.Single);

        // 4) 플레이어 재확인(씬마다 생성되는 구조 대비)
        Transform currentPlayer = player;
        if (currentPlayer == null)
        {
            var pObj = GameObject.FindWithTag("Player");
            if (pObj != null) currentPlayer = pObj.transform;
        }

        // 5) 스폰 이동
        if (currentPlayer != null)
        {
            var spawn = FindSpawnPoint(targetSpawnId);
            if (spawn != null)
                currentPlayer.position = spawn.position;
        }

        // 6) Fade In
        if (FadeController.Instance != null)
            yield return FadeController.Instance.FadeTo(0f, fadeInTime);

        // 7) 입력 잠금 해제
        if (currentPlayer != null)
        {
            var lock2 = currentPlayer.GetComponent<IInputLock>();
            lock2?.SetLocked(false);
        }

        _isTransitioning = false;
    }

    private Transform FindSpawnPoint(string targetSpawnId)
    {
        var spawns = GameObject.FindObjectsOfType<SpawnPoint>();
        foreach (var s in spawns)
        {
            if (s.SpawnId == targetSpawnId)
                return s.transform;
        }
        return spawns.Length > 0 ? spawns[0].transform : null;
    }
}
