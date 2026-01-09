using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverHandler : MonoBehaviour
{
    [Header("UI 패널 연결")]
    public GameObject gameOverUI; // 패배 창
    public GameObject victoryUI;  // 승리 창

    [Header("씬 이동 설정")]
    public string mainMenuSceneName = "MainMenu"; // 원래 씬 이름

    void Awake()
    {
        // 시작할 때 모든 UI 끄기
        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (victoryUI != null) victoryUI.SetActive(false);
        
        // 시간 정상화
        Time.timeScale = 1f;
    }

    // [패배] 플레이어가 죽었을 때 호출됨
    public void DisplayGameOver()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
            Time.timeScale = 0f; // 게임 일시정지
        }
    }

    // [승리] 적이 죽었을 때 호출됨
    public void DisplayVictory()
    {
        if (victoryUI != null)
        {
            victoryUI.SetActive(true);
            Time.timeScale = 0f; // 게임 일시정지
        }
    }

    // 다시 시작 버튼 기능
    public void OnClickRetry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 원래 씬(메인 메뉴)으로 돌아가는 기능
    public void BackToOriginalScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}