using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class PlayerDataToJson : MonoBehaviour
{
    public static PlayerDataToJson Instance;
    public PlayerData playerData;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SavePlayerDataToJson();
    }

    void Start()
    {
        //SavePlayerDataToJson();
    }
    string directoryPath = Path.Combine(
            Application.dataPath,
            "UI",
            "Inventory"
        );

    [ContextMenu("To Json Data")]
    void SavePlayerDataToJson()
    {
        string jsonData = JsonUtility.ToJson(playerData, true);

        

        // ⭐ 폴더 없으면 생성
        Directory.CreateDirectory(directoryPath);

        string filePath = Path.Combine(
            directoryPath,
            "playerData.json"
        );

        File.WriteAllText(filePath, jsonData);

        Debug.Log("저장 완료: " + filePath);
    }

    [ContextMenu("From Json Data")]
    void LoadPlayerDataFromJson()
    {
        // 데이터를 불러올 경로 지정
        string path = Path.Combine(directoryPath, "playerData.json");
        // 파일의 텍스트를 string으로 저장
        string jsonData = File.ReadAllText(path);
        // 이 Json데이터를 역직렬화하여 playerData에 넣어줌
        playerData = JsonUtility.FromJson<PlayerData>(jsonData);
    }

    
    public void SavePlayerDataToJsonCall(PlayerData data)
    {
        string jsonData = JsonUtility.ToJson(data, true);



        // ⭐ 폴더 없으면 생성
        Directory.CreateDirectory(directoryPath);

        string filePath = Path.Combine(
            directoryPath,
            "playerData.json"
        );

        File.WriteAllText(filePath, jsonData);

        Debug.Log("저장 완료: " + filePath);
    }

  
    public PlayerData LoadPlayerDataFromJsonCall()
    {
        // 데이터를 불러올 경로 지정
        string path = Path.Combine(directoryPath, "playerData.json");
        // 파일의 텍스트를 string으로 저장
        string jsonData = File.ReadAllText(path);
        // 이 Json데이터를 역직렬화하여 playerData에 넣어줌
        return JsonUtility.FromJson<PlayerData>(jsonData);
    }

    public ItemData CheckItemData(string name)
    {
        PlayerData data = LoadPlayerDataFromJsonCall();
        foreach (var itemDiff in data.items)
        {
            if (itemDiff.name == name)
            {
                return (ItemData)itemDiff;
            }
        }
        return null;
    }
    //count +1 or -1
    public void UpdateItemData(string name, int count)
    {
        PlayerData data = LoadPlayerDataFromJsonCall();
        bool NotHaveItem = true;
        foreach (var itemDiff in data.items)
        {
            if (itemDiff.name == name)
            {
                itemDiff.count += count;
                NotHaveItem = false;
                break;
            }
        }

        //새로운 아이템이라면
        if (NotHaveItem)
        {
            data.items.Add(new ItemData
            {
                name = name,
                count = count,
                value = 0 // 기본값 임시
            });
        }

        SavePlayerDataToJsonCall(data);
    }
}



[System.Serializable]
public class PlayerData
{
    public string name;
    public int age;
    public int level;
    public bool isDead;
    //public ItemData[] items;
    public List<ItemData> items = new List<ItemData>();
}

[System.Serializable]
public class ItemData
{
    public string name;
    public int count;
    public int value;
}