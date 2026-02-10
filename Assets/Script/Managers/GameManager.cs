using UnityEngine;
using UnityEngine.SceneManagement; // 중요: 씬 이동을 위해 꼭 필요함

public class GameManager : MonoBehaviour
{
    // 유니티 인스펙터 창에서 게임오버 UI 패널을 연결할 칸
    public GameObject gameOverUI; 

    // 게임오버 창을 띄우는 함수
    public void EndGame()
    {
        gameOverUI.SetActive(true); // 숨겨놨던 UI를 켬
        Time.timeScale = 0f;        // 게임 시간을 멈춤
    }

    // Retry 버튼에 연결할 함수
    public void RestartGame()
    {
        Time.timeScale = 1f;        // 멈췄던 시간을 다시 흐르게 함
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 현재 씬 다시 로드
    }

    // Exit 버튼에 연결할 함수
    public void ExitGame()
    {
        Application.Quit();         // 게임 종료
        Debug.Log("게임 종료됨");      // 에디터에서는 안 꺼지므로 로그로 확인
    }
}