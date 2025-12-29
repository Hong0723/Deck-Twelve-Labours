using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExplainItemToText : MonoBehaviour
{

    public static ExplainItemToText Instance { get; private set; }
    // 아이템 설명 관련
    [TextArea] public string itemDescription; // 인스펙터에서 설명 입력 가능
    public TextMeshProUGUI descriptionTextUI; // 설명이 표시될 UI Text (예: 화면 하단 텍스트 박스)

   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //Debug.Log("ExplainItemToText Start 실행");
        //Debug.Log($"Instance: {Instance}, descriptionTextUI: {descriptionTextUI}");
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;        
        DontDestroyOnLoad(gameObject);

        descriptionTextUI.gameObject.SetActive(false);//텍스트를 숨김상태로 시작
    }

    public void ShowDescription(string description)
    {
        
        if (descriptionTextUI != null)
        {
            //Debug.LogWarning($"[ItemManager] descriptionTextUI가 설정");
            descriptionTextUI.text = description;
            descriptionTextUI.gameObject.SetActive(true);
        }
        else
        {
            //Debug.LogWarning($"[ItemManager] descriptionTextUI가 설정되지 않음: {name}");
        }
    }

    //설명 숨기기 함수
    public void HideDescription()
    {
        
        if (descriptionTextUI != null)
        {
            descriptionTextUI.gameObject.SetActive(false);
        }        
    }
}
