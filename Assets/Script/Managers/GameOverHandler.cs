using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverHandler : MonoBehaviour
{
    public GameObject gameOverUI;   // GameOver/Panel
    public GameObject victoryUI;    // Victory/Panel
    public GameObject rewardUI;     // Reward/Panel
    [SerializeField] private GameObject battleInputRoot;
    private GameObject currentEnemy; // 제거할 몬스터 저장

    void Awake()
    {
        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (victoryUI != null) victoryUI.SetActive(false);
        if (rewardUI != null) rewardUI.SetActive(false);

        Time.timeScale = 1f;
    }

    // ===== 패배 =====
    public void DisplayGameOver()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    // ===== 승리 =====
    public void DisplayVictory(GameObject enemy)
    {
        currentEnemy = enemy;
        if (battleInputRoot != null)
        battleInputRoot.SetActive(false);   // 🔥 입력 전체 차단
        if (victoryUI != null)
        {
            victoryUI.SetActive(true);
        }
    }

    // Victory → Reward 버튼
    public void OnClickVictoryContinue()
    {
        if (victoryUI != null) victoryUI.SetActive(false);
        if (rewardUI != null) rewardUI.SetActive(true);
        Time.timeScale = 0f;
          Debug.Log("Reward Continue Clicked");
    }

    // Reward → 게임 복귀 버튼
    public void OnClickRewardContinue()
    {

        if (battleInputRoot != null)
        battleInputRoot.SetActive(true);
        if (rewardUI != null) rewardUI.SetActive(false);

        Time.timeScale = 1f;

        if (currentEnemy != null)
            currentEnemy.SetActive(false); // 몬스터 제거
                // 잡은 몬스터 기록
    if (DeliverBattleData.MonsterInfo != null)
    {
        GlobalMonsterState.MarkAsDefeated(
            DeliverBattleData.MonsterInfo.monsterName
        );
    }

    SceneManager.LoadScene("Game Scene");
      Debug.Log("Reward Continue Clicked");
    }

    // Retry 버튼
    public void OnClickRetry()
    {
        Time.timeScale = 1f;

    GameSessionManager.ResetAll();  

    SceneManager.LoadScene("Start Scene"); 
    }

    public void OnClickExit()
    {
        Application.Quit();
    }
}