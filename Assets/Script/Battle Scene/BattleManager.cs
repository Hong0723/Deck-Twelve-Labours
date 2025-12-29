using UnityEngine;
using System.Collections;
using TMPro;

public enum BattleState
{
    PlayerTurn,
    EnemyTurn,
    Victory,
    Defeat
}

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    public BattleState state;
    public Player player;
    public Enemy enemy;
    public TextMeshProUGUI turnText;
    public CardSystem cardSystem;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        cardSystem.Init();
        cardSystem.Draw(5);
        DebugHand();

        state = BattleState.PlayerTurn;
        enemy.OnPlayerTurnStart();   // ✅ 첫 턴 Defense 반영
        UpdateTurnUI();

        Debug.Log("Player Turn 시작");
        Debug.Log($"[Enemy Intent] {enemy.nextAction}");
    }

    void Update()
    {
        // 테스트용 공격
        if (state == BattleState.PlayerTurn && Input.GetKeyDown(KeyCode.Space))
        {
            enemy.TakeHitFromPlayer(10);
        }
    }

    public void EndPlayerTurn()
    {
        if (state != BattleState.PlayerTurn)
            return;

        state = BattleState.EnemyTurn;
        UpdateTurnUI();
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        Debug.Log("Enemy Turn 시작");

        enemy.OnEnemyTurnStart();   // ✅ Shield / Defense 정리

        yield return new WaitForSeconds(1f);

        enemy.TakeTurn();

        yield return new WaitForSeconds(1f);

        if (player.IsDead())
        {
            state = BattleState.Defeat;
            UpdateTurnUI();
            yield break;
        }

        state = BattleState.PlayerTurn;
        enemy.OnPlayerTurnStart(); // ✅ 다음 행동 반영
        UpdateTurnUI();

        Debug.Log($"[Enemy Intent] {enemy.nextAction}");
    }

    void UpdateTurnUI()
    {
        if (!turnText) return;

        turnText.text = state switch
        {
            BattleState.PlayerTurn => "PLAYER TURN",
            BattleState.EnemyTurn => "ENEMY TURN",
            BattleState.Victory => "VICTORY",
            BattleState.Defeat => "DEFEAT",
            _ => ""
        };
    }

    void DebugHand()
    {
        if (cardSystem == null)
        {
            Debug.LogWarning("CardSystem이 BattleManager에 연결 안됨");
            return;
        }

        if (cardSystem.hand == null)
        {
            Debug.LogWarning("CardSystem.hand가 null임 (CardSystem 스크립트 확인)");
            return;
        }

        string s = "HAND: ";
        foreach (var c in cardSystem.hand)
        {
            if (c == null) { s += "(null) "; continue; }
            s += c.cardName + " ";
        }
        Debug.Log(s);
    }
}
