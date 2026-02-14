using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public enum EnemyActionType
{
    Attack,
    Shield,
    Heal,
    Defense
}

public class Enemy : MonoBehaviour, IDamageable
{
    public int maxHP = 40;
    public int currentHP;
    public int shield;
    public bool CounterReady;
    public int defenseReduce = 40;
    public int counterDamage = 4;
    public int attackDamage;
    public EnemyActionType nextAction;
    public HPBar hpBar;
    public Animator animator;
    private MonsterSkill effectScript;
    private GameObject newVisual;
    [SerializeField] private IntentUI intentUI;
    
    
    void Start()
    {
        currentHP = maxHP;
        shield = 0;


        if (DeliverBattleData.MonsterInfo)
        {
            MonsterType monsterInfo = DeliverBattleData.MonsterInfo;
            maxHP = monsterInfo.maxHP;
            currentHP = maxHP;
            shield = monsterInfo.shield;
            defenseReduce = monsterInfo.defenseReduce;
            counterDamage = monsterInfo.counterDamage;
            attackDamage = monsterInfo.AttackDamage;
            GameObject monsterObj = FindByTagAndName("Monster", monsterInfo.name);
            ReplaceVisual(this.gameObject, monsterObj);            
            effectScript = newVisual.GetComponent<MonsterSkill>();
            //-----
        }


        DecideNextAction();
        UpdateHPBar();
    }


    public void OnPlayerTurnStart()
    {
    }

    public void OnEnemyTurnStart()
    {
        shield = 0;
        UpdateHPBar();
    }


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
                BattleManager.Instance.player.TakeDamage(attackDamage);
                BattleManager.Instance.player.HurtedAnimation();            
                break;

            case EnemyActionType.Shield:
                shield += 6;
                SpecialAnimation();
                break;

            case EnemyActionType.Heal:
                currentHP = Mathf.Min(maxHP, currentHP + 5);
                GetComponent<EnemyStatus>().TakeDamage(-5);//체력회복
                HealAnimation();
                break;

            case EnemyActionType.Defense:
                DefenseAnimation();
                CounterReady = true;
                break;
        }

        UpdateHPBar();
    }


    public void TakeDamage(int damage)
    {
        Debug.Log("TakeDamage 호출됨, 데미지: " + damage);
        TakeHitFromPlayer(damage);
    }

    public void TakeHitFromPlayer(int damage)
    {       

        int finalDamage = damage;

        if (CounterReady)
        {
            Debug.Log("Defense Counter 발동");
            finalDamage = Mathf.RoundToInt(damage * 0.5f);
            BattleManager.Instance.player.TakeDamage(counterDamage);
            BattleManager.Instance.player.SetDefensed(true);
                   
        }

        int absorbed = Mathf.Min(shield, finalDamage);
        shield -= absorbed;
        finalDamage -= absorbed;

        GetComponent<EnemyStatus>().TakeDamage(finalDamage);//����������� �ݿ��ؼ� ���

        currentHP = Mathf.Max(currentHP - finalDamage, 0);
        UpdateHPBar();
        HurtedAnimation();

        if (currentHP <= 0)
        {
        gameObject.SetActive(false);
        }
    }


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
        if(intentUI != null){
         intentUI.UpdateIntent();
        }
    }


    void UpdateHPBar()
    {
        if (hpBar)
            hpBar.Set(currentHP, maxHP, shield);
    }

    public void Attack1Animation()
    {
        effectScript.EffectAttack();
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


    GameObject FindByTagAndName(string tag, string name)
    {
        //Debug.Log($"[FindByTagAndName] �˻� ���� | tag: {tag}, name: {name}");

        GameObject[] objs;// = GameObject.FindGameObjectsWithTag(tag);
        try
        {
            objs = GameObject.FindGameObjectsWithTag(tag);
        }
        catch
        {
            //Debug.LogError($"[FindByTagAndName] �������� �ʴ� Tag: {tag}");
            return null;
        }
        foreach (GameObject obj in objs)
        {
            if (obj.name == name)
            {
                //Debug.Log($"[FindByTagAndName] �߰�: {obj.name}");
                return obj;
            }
        }

        //Debug.LogWarning($"[FindByTagAndName] �ش� �̸��� ���� ������Ʈ ���� | name: {name}");
        return null;
    }


    //Enemy������Ʈ �ڽ����� �˸��� ���� ������
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

    public void SetCurrentHP(int hp)
    {
        currentHP = hp; 
    }

    public string GetIntentDescription() // 적 패턴 정보 UI
{
    switch (nextAction)
    {
        case EnemyActionType.Attack:
            return $" Attack : {attackDamage} Damage";

        case EnemyActionType.Shield:
            return " Shield +6";

        case EnemyActionType.Heal:
            return " Heal Hp +5";

        case EnemyActionType.Defense:
            return $" Defense Mode\n (Damage 50% decrease, counter {counterDamage})";

        default:
            return "";
    }
}
}

