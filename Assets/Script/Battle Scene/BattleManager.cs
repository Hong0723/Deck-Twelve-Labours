using UnityEngine;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    public enum BattleState
    {
        Start,
        PlayerTurn,
        EnemyTurn,
        Victory,
        Defeat
    }   
    
    public static BattleManager Instance;

    [Header("Scene References")]
    [SerializeField] private GameObject enemyObject;   // Enemy 타입 의존 완전 제거
    [SerializeField] public Player player; //EnemyLoad에서 접근하기위해 public으로 바꿔놨습니다. 김동주
    [SerializeField] private IntentUI intentUI;

    private BattleState state;

    public bool IsVictory => state == BattleState.Victory; // 경빈 추가 퀘스트에서 승리 판정
    public GameObject EnemyObject => enemyObject; // 경빈 추가 퀘스트에서 오브젝트 id 불러오는 인터페이스

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        state = BattleState.Start;
        StartCoroutine(SetupBattle());
        Enemy enemy = enemyObject.GetComponent<Enemy>();
        intentUI.SetEnemy(enemy);
    }

    private IEnumerator SetupBattle()
    {
        yield return new WaitForSeconds(0.5f);
        state = BattleState.PlayerTurn;

        // 적에게 플레이어 턴 시작 알림 (있으면 실행, 없으면 무시)
        enemyObject?.SendMessage(
            "OnPlayerTurnStart",
            SendMessageOptions.DontRequireReceiver
        );
    }

    public void OnPlayerActionEnd()
    {
        if (state != BattleState.PlayerTurn)
            return;
        // 플레이어 턴 종료 → Defense 해제
        enemyObject?.SendMessage(
            "OnPlayerTurnEnd",
            SendMessageOptions.DontRequireReceiver
        );

        state = BattleState.EnemyTurn;
        StartCoroutine(EnemyTurn());
    }

    public void WinBattle()
    {
        if (state == BattleState.Victory)
        return;
        state = BattleState.Victory;
        GameOverHandler handler = FindObjectOfType<GameOverHandler>();
        handler.DisplayVictory(enemyObject);
    }
    private IEnumerator EnemyTurn()
    {
        enemyObject?.SendMessage(
            "OnEnemyTurnStart",
            SendMessageOptions.DontRequireReceiver
        );

        enemyObject?.SendMessage(
            "TakeTurn",
            SendMessageOptions.DontRequireReceiver
        );

        yield return new WaitForSeconds(0.5f);

        // 전역 HP 기준으로 패배 판정
        if (GlobalPlayerHP.CurrentHP <= 0)
        {
            state = BattleState.Defeat;
            GlobalPlayerHP.ResetOnGameOver();
            yield break;
        }

        state = BattleState.PlayerTurn;

        enemyObject?.SendMessage(
            "OnPlayerTurnStart",
            SendMessageOptions.DontRequireReceiver
        );
    }
}
