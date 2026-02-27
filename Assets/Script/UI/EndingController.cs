using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndingSceneController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI endingText;

    private string[] lines =
    {
        "열두 과업을 완수한 자\n이제 인간을 넘어 신들의 반열에 오른다.",
        "고난을 이겨낸 자만이 영웅이라 불린다.\n오늘, 헤라클레스는 그 이름을 증명하였다.",
        "속죄는 끝났다.\n남은 것은 영광뿐이다."
    };

    private int currentIndex = 0;
    private bool isFinished = false;

    void Start()
    {
        endingText.text = lines[currentIndex];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isFinished)
            {
                currentIndex++;

                if (currentIndex < lines.Length)
                {
                    endingText.text = lines[currentIndex];
                }
                else
                {
                    isFinished = true;
                    SceneManager.LoadScene("Start Scene");
                }
            }
        }
    }
}