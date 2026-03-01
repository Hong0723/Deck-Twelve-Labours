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

            // ✅ [추가] 이 몬스터가 가진 배경을 CombatContext에 저장
            // (MonsterBattleBackground는 몬스터 Inspector에서 배경 Sprite 지정하는 컴포넌트)
            var mbg = GetComponent<MonsterBattleBackground>();
            if (mbg != null)
                CombatContext.Instance.SetBackground(mbg.battleBackground);
            else
                CombatContext.Instance.SetBackground(null); // 없으면 BattleScene에서 default 사용

            // 기존 씬 전환 유지
            SceneTransitionManager.Instance.TransitionTo(sceneName);
        }
    }
}