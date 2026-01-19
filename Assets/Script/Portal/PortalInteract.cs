using UnityEngine;

public class PortalInteract : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private GameObject interactUI;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

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
        if (interactUI != null) interactUI.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        _playerInRange = false;
        _player = null;
        if (interactUI != null) interactUI.SetActive(false);
    }
}
