using UnityEngine;

public class ChangeSceneOnHitpt : MonoBehaviour
{
    [SerializeField] private string sceneName = "Battle Scene";
    [SerializeField] private MonsterType MonsterInfo;

    private bool _triggered;

    private void Start()
    {
        if (GlobalMonsterState.IsDefeated(MonsterInfo.monsterName))
            gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_triggered) return;

        if (collision.collider.CompareTag("Player"))
        {
            _triggered = true;

            GlobalWorldState.lastPlayerPosition = collision.transform.position;
            GlobalWorldState.hasSavedPosition = true;

            DeliverBattleData.MonsterInfo = MonsterInfo;

            // ✅ 바뀐 부분
            SceneTransitionManager.Instance.TransitionTo(sceneName);
        }
    }
}