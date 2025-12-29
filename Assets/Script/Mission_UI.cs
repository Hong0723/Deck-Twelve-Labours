
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public Transform[] characters;      // 캐릭터들 (Char1, Char2)
    public Vector3 cameraPosition;      // 카메라 중앙 위치
    public Vector3 offscreenPosition;   // 카메라 밖 위치

    public Vector3 textPosition;
    public Transform text_box;

    public TextMeshProUGUI text;
    private int currentIndex = -1;      // 현재 등장 중인 캐릭터 인덱스

    void Start()
    {
        // UI Scene이 아닐 경우 실행 금지
        if (SceneManager.GetActiveScene().name != "UI Scene")
            return;

        // 모든 캐릭터 화면 밖으로 이동

        text_box.position = offscreenPosition;
        text.text = "";
        foreach (var c in characters)
        {

            c.position = offscreenPosition;
        }
    }

    void Update()
    {
        // UI Scene에서만 동작
        if (SceneManager.GetActiveScene().name != "UI Scene")
            return;

        // 스페이스 또는 엔터
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            HandleCharacterSequence();
        }
    }

    void HandleCharacterSequence()
    {
        currentIndex++;

        // 마지막 이후 → Game Scene 로드
        switch (currentIndex)
        {
            case 0 :
                characters[0].position = cameraPosition;
                text_box.position = textPosition;
                text.text = "hello!";
                break;
            case 1 :
                text.text = "world!";
                break;
            case 2 :
                characters[0].position = offscreenPosition;
                characters[1].position = cameraPosition;
                text.text = "labouls!";
                break;
            case 3 :
                text.text = "let's go!";
                break;
            case 4 :
                SceneManager.LoadScene("Game Scene");
                break;
            default :
                SceneManager.LoadScene("Game Scene");
                break;

        }
        return;
    }
}
