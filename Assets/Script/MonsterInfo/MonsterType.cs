using UnityEngine;

[CreateAssetMenu(fileName = "MonsterType", menuName = "MonsterInfo/MonsterType")]
public class MonsterType : ScriptableObject
{
    [Header("Basic Stats")]
    public string monsterName;
    public int maxHP;
    public int shield;
    public int defenseReduce;
    public int counterDamage;
    public int block;
    public int AttackDamage;
    public int Heal;

    [Header("Special Rulse")]
    public int reviveTimes; // 2과업 히드라용
    public bool heavyDefense; // 3과업 사슴용
    public int weakenTurn; // 4과업 멧돼지용
    public int weakenHP; 
    public int invincibleTurns; // 6과업 새용
    public bool surviveMode; // 11과업 아틀라스용
    public bool isFinalBoss; // 엔딩씬 용

    [Header("Set UP")]
    public Vector3 SpawnPosition;//BattleScene 몬스터 스폰 위치 오프셋
}
