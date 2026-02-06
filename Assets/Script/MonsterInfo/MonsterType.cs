using UnityEngine;

[CreateAssetMenu(fileName = "MonsterType", menuName = "MonsterInfo/MonsterType")]
public class MonsterType : ScriptableObject
{
    //Battle씬의 Enemy오브젝트에 붙어있는 Enemy스크립트에서
    //사용할 스크립트 오브젝트
    public string monsterName;
    public int maxHP;//몬스터,플레이어 최대체력
    public int shield;//몬스터가 쉴드가지고있다면 몇인지
    public int defenseReduce;//몬스터 속성
    public int counterDamage;//몬스터 속성
    public int block;//플레이어 속성
    public int AttackDamage;//플레이어 몬스터 공격력
}
