using UnityEngine;
using UnityEngine.SceneManagement; // 이 줄이 꼭 있어야 합니다.

public class SceneChanger : MonoBehaviour
{
    // 이동하고 싶은 씬 이름을 유니티 인스펙터 창에서 직접 입력하세요.
    public string sceneName;

    public void ContinueToNextScene()
    {
        // 1. 게임오버 시 시간이 멈췄을 수 있으니 다시 흐르게 합니다.
        Time.timeScale = 1f;

        // 2. 지정한 이름의 씬을 불러옵니다.
        SceneManager.LoadScene(sceneName);
    }
}