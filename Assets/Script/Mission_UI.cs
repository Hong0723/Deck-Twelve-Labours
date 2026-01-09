
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

    public Button button;


    
    void Start()
    {
        // UI Scene이 아닐 경우 실행 금지
        if (SceneManager.GetActiveScene().name != "UI Scene")
            return;

        // 모든 캐릭터 화면 밖으로 이동

        button.gameObject.SetActive(false);
        text_box.position = offscreenPosition;
        text.text = "";
        foreach (var c in characters)
        {

            c.position = offscreenPosition;
<<<<<<< HEAD

=======
>>>>>>> 299f1c3cceb54b517fcc3ec8e22e163acc86f5fc
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
            button.gameObject.SetActive(true);
            HandleCharacterSequence();
        }
    }

    public void HandleCharacterSequence()
    {
        currentIndex++;


        // 마지막 이후 → Game Scene 로드
        switch (currentIndex)
        {
            case 0 :
                characters[0].position = cameraPosition;
                text_box.position = textPosition;
<<<<<<< HEAD
                text.text = "헤라클레스.\n네가 저지른 죄는 아직 끝나지 않았다.";
                break;
            case 1 :
                text.text = "신들은 너에게 자비를 주지 않는다.\n오직 시련을 통한 속죄만이 허락된다.!";
=======
                text.text = "hello!";
                break;
            case 1 :
                text.text = "world!";
>>>>>>> 299f1c3cceb54b517fcc3ec8e22e163acc86f5fc
                break;
            case 2 :
                characters[0].position = offscreenPosition;
                characters[1].position = cameraPosition;
<<<<<<< HEAD
                text.text = "이것이 네가 짊어질 운명이다.\n열두 가지 시련을 모두 완수하라.";
                break;
            case 3 :
                text.text = "하나라도 거부하거나 실패한다면,\n신들은 너를 영원히 버릴 것이다.";
=======
                text.text = "labouls!";
                break;
            case 3 :
                text.text = "let's go!";
>>>>>>> 299f1c3cceb54b517fcc3ec8e22e163acc86f5fc
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
