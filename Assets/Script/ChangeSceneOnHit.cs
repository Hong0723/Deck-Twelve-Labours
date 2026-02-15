using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnHitpt : MonoBehaviour
{
    [SerializeField] private string sceneName = "Battle Scene";
    [SerializeField] private MonsterType MonsterInfo;//��Ʋ������ �ο�� �� ���� ����



    void Start()
    {
    if (GlobalMonsterState.IsDefeated(MonsterInfo.monsterName))
    {
        gameObject.SetActive(false);
    }
    }   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
        GlobalWorldState.lastPlayerPosition = collision.transform.position;
        GlobalWorldState.hasSavedPosition = true;

        DeliverBattleData.MonsterInfo = MonsterInfo;
        SceneManager.LoadScene("Battle Scene");
        }
    }
}
