using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
public class GameOverHandler : MonoBehaviour
{
    public GameObject gameOverUI;   // GameOver/Panel
    public GameObject victoryUI;    // Victory/Panel
    public GameObject rewardUI;     // Reward/Panel
    public GameObject rewardAnimationObj; //리워드 보상 연출
    //public AudioClip rewardclip;        // 리워드 재생할 사운드
    [SerializeField] private GameObject battleInputRoot;
    private GameObject currentEnemy; // 제거할 몬스터 저장

    void Awake()
    {
        if (Interactions.Instance != null)
            Interactions.Instance.SetInputLocked(false);

        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (victoryUI != null) victoryUI.SetActive(false);
        if (rewardUI != null) rewardUI.SetActive(false);
        if (rewardAnimationObj != null) rewardAnimationObj.SetActive(false);

        Time.timeScale = 1f;
    }

    // ===== 패배 =====
    public void DisplayGameOver()
    {
        if (Interactions.Instance != null)
            Interactions.Instance.SetInputLocked(true);

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
        if (Interactions.Instance != null)
            Interactions.Instance.SetInputLocked(true);

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
        StartCoroutine(RewardCoroutine());
        //if (rewardUI != null) rewardUI.SetActive(true);
        //Time.timeScale = 0f; 애니메이션 재생시켜야해서 주석했어요
          Debug.Log("Reward Continue Clicked");
    }

    // Reward → 게임 복귀 버튼
    public void OnClickRewardContinue()
    {
        if (Interactions.Instance != null)
            Interactions.Instance.SetInputLocked(false);

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
    DOTween.KillAll(); 
    SceneManager.LoadScene("Game Scene");
      Debug.Log("Reward Continue Clicked");
    }

    // Retry 버튼
    public void OnClickRetry()
    {
    if (Interactions.Instance != null)
        Interactions.Instance.SetInputLocked(false);
        
    GameSessionManager.ResetGameSession(); 
    Time.timeScale = 1f;
    DOTween.KillAll();   
    SceneManager.LoadScene("Start Scene"); 
    }

    public void OnClickExit()
    {
        Application.Quit();
    }

    IEnumerator RewardCoroutine()
    {
        rewardAnimationObj.SetActive(true);
        // 1. 보상 애니메이션 실행
        rewardAnimationObj.GetComponent<Animator>().SetTrigger("Play");

        AudioSource audioSource = rewardAnimationObj.GetComponent<AudioSource>();
        audioSource.Play();

        // 2. 애니메이션 시간만큼 대기
        yield return new WaitForSecondsRealtime(2.0f);

        rewardAnimationObj.SetActive(false);
        // 3. 보상창 활성화
        rewardUI.SetActive(true);
    }
}
