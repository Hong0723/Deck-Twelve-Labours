using UnityEngine;

public class PortalInteract : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private GameObject interactUI;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Quest Gate")]
    [Tooltip("true면 1~11 과업을 모두 완료해야 포탈 사용 가능")]
    [SerializeField] private bool requireLabours1To11 = true;

    [Header("Destination")]
    [SerializeField] private string targetSceneName;
    [SerializeField] private string targetSpawnId = "Default";
    [SerializeField] private float fadeOutTime = 0.35f;
    [SerializeField] private float fadeInTime = 0.35f;

    private bool _playerInRange;
    private Transform _player;

    private void Awake()
    {
        if (interactUI != null) interactUI.SetActive(false);
    }

    private void Update()
    {
        if (!_playerInRange) return;

        if (Input.GetKeyDown(interactKey))
        {
            // ✅ 퀘스트 조건 체크 (1~11 완료 전엔 막기)
            if (requireLabours1To11 && !IsPortalUnlockedByQuest())
            {
                int done = GetCompletedLabours1To11CountSafe();
                Debug.Log($"[Portal] 잠김: 1~11 과업 완료 필요 ({done}/11)");

                // 원하면 여기서 잠김 UI/사운드/토스트 띄우기
                return;
            }

            _playerInRange = false;
            if (interactUI != null) interactUI.SetActive(false);

            PortalTransition.Instance.StartTransition(
                player: _player,
                targetSceneName: targetSceneName,
                targetSpawnId: targetSpawnId,
                fadeOutTime: fadeOutTime,
                fadeInTime: fadeInTime
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        _playerInRange = true;
        _player = other.transform;

        // ✅ 완료 전이면 안내 UI를 숨기고 싶다면(기본)
        if (interactUI != null)
        {
            bool show = !requireLabours1To11 || IsPortalUnlockedByQuest();
            interactUI.SetActive(show);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        _playerInRange = false;
        _player = null;
        if (interactUI != null) interactUI.SetActive(false);
    }

    // ----------------------------
    // Quest Gate Helpers
    // ----------------------------

    private bool IsPortalUnlockedByQuest()
    {
        var q = QuestUIController.Instance;
        if (q == null) return false;
        return q.AreLabours1To11Completed();
    }

    private int GetCompletedLabours1To11CountSafe()
    {
        var q = QuestUIController.Instance;
        if (q == null) return 0;
        return q.GetCompletedLabours1To11Count();
    }
}