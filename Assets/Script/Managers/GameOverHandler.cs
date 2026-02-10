using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverHandler : MonoBehaviour
{
    public GameObject gameOverUI; // 패배창 패널
    public GameObject victoryUI;  // 승리창 패널
    public string nextSceneName;  // 계속하기 누를 때 갈 씬 이름

    void Awake()
    {
        // 시작할 때 모든 창을 끄고 시간은 정상(1)으로 설정
        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (victoryUI != null) victoryUI.SetActive(false);
        Time.timeScale = 1f; 
    }

    public void DisplayGameOver()
    {
        if (gameOverUI != null) {
            gameOverUI.SetActive(true);
            Time.timeScale = 0f; // 게임 멈춤
        }
    }

    public void DisplayVictory()
    {
        if (victoryUI != null) {
            victoryUI.SetActive(true);
            Time.timeScale = 0f; // 게임 멈춤
        }
    }

    public void OnClickRetry() // 다시하기 버튼용
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnClickContinue() // 계속하기 버튼용 (씬 이동)
    {
        Time.timeScale = 1f;
        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
    }

    public void OnClickExit() // 종료 버튼용
    {
        Application.Quit();
    }
}