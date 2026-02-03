using UnityEngine;

public class BattleBGMStarter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        BGMManager.Instance.PlayBattleBGM();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
