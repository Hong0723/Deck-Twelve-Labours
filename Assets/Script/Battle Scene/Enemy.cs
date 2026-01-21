using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EnemyActionType
{
    Attack,
    Shield,
    Heal,
    Defense
}

public class Enemy : MonoBehaviour
{
    public int maxHP = 40;
    public int currentHP;
    public int shield;

    public bool isDefending;

    public int defenseReduce = 40;
    public int counterDamage = 4;

    public EnemyActionType nextAction;
    public HPBar hpBar;

    public Animator animator;
    

    private MonsterSkill effectScript;
    private GameObject newVisual;//화면밖에 적을 복제


    void Start()
    {
        currentHP = maxHP;
        shield = 0;
        isDefending = false;
        

        if (DeliverBattleData.MonsterInfo)
        {
            //스크립터블 오브젝트에서 몬스터 정보를 가져옵니다
            MonsterType monsterInfo = DeliverBattleData.MonsterInfo;
            maxHP = monsterInfo.maxHP;
            currentHP = maxHP;
            shield = monsterInfo.shield;
            defenseReduce = monsterInfo.defenseReduce;
            counterDamage = monsterInfo.counterDamage;
            GameObject monsterObj = FindByTagAndName("Monster", monsterInfo.name);
            ReplaceVisual(this.gameObject, monsterObj);
            /*
            Transform effectStartPos = transform.Find("EffectStartPos");
            effectScript = effectStartPos.GetComponent<MonsterSkill>();
            */
            //effectScript = GetComponentInChildren<MonsterSkill>();
            effectScript = newVisual.GetComponent<MonsterSkill>();
            //-----
        }
        

        DecideNextAction();
        UpdateHPBar();
    }

    // =========================
    // 턴 훅
    // =========================
    public void OnPlayerTurnStart()
    {
        if (nextAction == EnemyActionType.Defense)
        {
            isDefending = true;
            Debug.Log("Enemy 방어 태세");
        }
    }

    public void OnEnemyTurnStart()
    {
        shield = 0;
        isDefending = false;
        UpdateHPBar();
    }

    // =========================
    // Enemy 행동
    // =========================
    public void TakeTurn()
    {
        ExecuteAction();
        DecideNextAction();
    }

    void ExecuteAction()
    {
        switch (nextAction)
        {
            case EnemyActionType.Attack:
                Attack1Animation();
                BattleManager.Instance.player.TakeDamage(8);
                BattleManager.Instance.player.HurtedAnimation();//플레이어 피격애니메이션                
                break;

            case EnemyActionType.Shield:
                shield += 6;
                SpecialAnimation();//시전애니메이션
                break;

            case EnemyActionType.Heal:
                currentHP = Mathf.Min(maxHP, currentHP + 5);
                HealAnimation();//시전애니메이션
                break;

            case EnemyActionType.Defense:
                DefenseAnimation();//시전애니메이션
                // 실행 없음 (상태용)
                break;
        }

        UpdateHPBar();
    }

    // =========================
    // Player 공격 대응
    // =========================
    public void TakeHitFromPlayer(int damage)
    {
        int finalDamage = damage;

        if (isDefending)
        {
            finalDamage = Mathf.Max(damage - defenseReduce, 0);
            BattleManager.Instance.player.TakeDamage(counterDamage);
            Debug.Log("Defense 반격 발동");

            BattleManager.Instance.player.SetDefensed(true);
            //BattleManager.Instance.player.resetHurtedTrigger();
            //BattleManager.Instance.player.resetAttakcTrigger();            
        }

        int absorbed = Mathf.Min(shield, finalDamage);
        shield -= absorbed;
        finalDamage -= absorbed;

        currentHP = Mathf.Max(currentHP - finalDamage, 0);
        UpdateHPBar();
        HurtedAnimation();
    }

    // =========================
    // 패턴
    // =========================
    void DecideNextAction()
    {
        List<EnemyActionType> pool = new()
        {
            EnemyActionType.Attack,
            EnemyActionType.Shield,
            EnemyActionType.Defense
        };

        if (currentHP < maxHP * 0.8f)
            pool.Add(EnemyActionType.Heal);

        nextAction = pool[Random.Range(0, pool.Count)];
    }

    void UpdateHPBar()
    {
        if (hpBar)
            hpBar.Set(currentHP, maxHP, shield);
    }

    public void Attack1Animation()
    {
        effectScript.EffectAttack();//히드라만 구현
        animator.SetTrigger("Attack1");
    }

    public void SpecialAnimation()
    {
        effectScript.EffectShield();
        animator.SetTrigger("Special");
    }
    public void HealAnimation()
    {
        effectScript.EffectHeal();
        animator.SetTrigger("Special");
    }    
    public void DefenseAnimation()
    {
        effectScript.EffectDefense();
        animator.SetTrigger("Special");
    }
    public void HurtedAnimation()
    {
        animator.SetTrigger("Hurted");
    }

    //EnemyWaitingRoom에 있는 몬스터 오브젝트 서치(Tag(Monster)->이름 순으로 찾음)
    GameObject FindByTagAndName(string tag, string name)
    {
        //Debug.Log($"[FindByTagAndName] 검색 시작 | tag: {tag}, name: {name}");

        GameObject[] objs;// = GameObject.FindGameObjectsWithTag(tag);
        try
        {
            objs = GameObject.FindGameObjectsWithTag(tag);
        }
        catch
        {
            //Debug.LogError($"[FindByTagAndName] 존재하지 않는 Tag: {tag}");
            return null;
        }
        foreach (GameObject obj in objs)
        {
            if (obj.name == name)
            {
                //Debug.Log($"[FindByTagAndName] 발견: {obj.name}");
                return obj;
            }
        }

        //Debug.LogWarning($"[FindByTagAndName] 해당 이름을 가진 오브젝트 없음 | name: {name}");
        return null;
    }


    //Enemy오브젝트 자식으로 알맞은 적이 생성됨
    public void ReplaceVisual(GameObject target, GameObject visualSource)
    {
        if (!target || !visualSource)
            return;

        Transform old = target.transform.Find("VisualRoot");
        if (old) Object.Destroy(old.gameObject);

        newVisual = Object.Instantiate(visualSource, target.transform);
        newVisual.name = "VisualRoot";
        
        newVisual.transform.localPosition = Vector3.zero;
        newVisual.transform.localRotation = Quaternion.identity;

        Vector3 srcWorldScale = visualSource.transform.lossyScale;
        Vector3 parentWorldScale = target.transform.lossyScale;

        newVisual.transform.localScale = new Vector3(
            srcWorldScale.x / parentWorldScale.x,
            srcWorldScale.y / parentWorldScale.y,
            srcWorldScale.z / parentWorldScale.z
        );

        //Debug.Log($"[ReplaceVisual] world scale matched: {srcWorldScale}");
        animator = newVisual.GetComponent<Animator>();
    }

    public GameObject GetnewVisualObj()
    {
        return newVisual;
    }
}

