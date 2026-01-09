using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnHitpt : MonoBehaviour
{
    [SerializeField] private string sceneName = "Battle Scene";
    [SerializeField] private MonsterType MonsterInfo;//배틀씬에서 싸우게 될 몬스터 정보

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            //static변수에 값 저장
            DeliverBattleData.MonsterInfo = MonsterInfo;            
            SceneManager.LoadScene(sceneName);
        }
    }
}
