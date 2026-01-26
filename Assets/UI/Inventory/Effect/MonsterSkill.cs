using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EffectData
{
    public string skillNameHaveImpact;//스킬이름과 스킬의 후속 애니메이션 유무를 하나로 저장
    public EffectObjBol data;   
}

[System.Serializable]
public class EffectObjBol
{    
    public GameObject prefab;
    public bool haveImpaact = false;
}

public class MonsterSkill : MonoBehaviour
{
    public List<EffectData> effects;//인스펙터 등록용
    private Dictionary<string, EffectObjBol> effectMap;//코드내 서치용, 원본 오브젝트
    //private Dictionary<string, EffectObjBol> effectMapRef;//Instantiate 후 여기에 저장,후에는 Destroy관리
    private Dictionary<string, GameObject> effectMapRef; // 생성된 인스턴스


    //public GameObject skillStartPos;//스킬 시작점
    public GameObject attackSocket;//스킬 시작점
    public GameObject healSocket;//스킬 시작점
    public GameObject defenseSocket;//스킬 시작점
    public GameObject shieldSocket;//스킬 시작점
    public GameObject skillEndPos;//플레이어가 스킬 맞는 지점
    private bool isMoveAble;
    public float speed;

    //몬스터마다 크기가 다르니까 HP바 위치 재설정
    public GameObject head;    
    Camera cam;
    public GameObject hpBar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isMoveAble = false;
        speed = 5f;
        effectMap = new Dictionary<string, EffectObjBol>();
        effectMapRef = new Dictionary<string, GameObject>();
        foreach (var e in effects)
        {
            if (!effectMap.ContainsKey(e.skillNameHaveImpact))
                effectMap.Add(e.skillNameHaveImpact, e.data);
        }
        /*
        Debug.Log("==== effectMap 최종 등록 키 목록 ====");
        foreach (var key in effectMap.Keys)
        {
            Debug.Log("등록된 키: " + key);
        }*/
        cam = Camera.main;
        Vector3 offset = new Vector3(0,10,0);        // 머리 위 보정
        Vector3 worldPos = head.transform.position + offset;
        Vector3 screenPos = cam.WorldToScreenPoint(worldPos);
        hpBar.transform.position = screenPos;

    }

    // Update is called once per frame
    void Update()
    {
        if (isMoveAble)
        {
            if (!effectMapRef["Attack"])
            {
                return;
            }

            effectMapRef["Attack"].transform.position = Vector3.Lerp(effectMapRef["Attack"].transform.position, skillEndPos.transform.position, Time.deltaTime * speed);

            if (Vector3.Distance(effectMapRef["Attack"].transform.position, skillEndPos.transform.position) < 0.5f)
            {
                EffectAttackImpact();
                isMoveAble = false;
            }
        }
    }

    public void EffectAttack()
    {
        if (effectMap["Attack"].haveImpaact)
        {//플레이어가 스킬에 대한 피격애니메이션이 있을때
            Debug.Log("Attack 생성");
            effectMapRef["Attack"] = Instantiate(effectMap["Attack"].prefab, attackSocket.transform.position, Quaternion.identity);
            Animator animator = effectMapRef["Attack"].GetComponent<Animator>();
            animator.SetBool("Effect", true);
            isMoveAble = true;
        }
        else
        {//없을때
            effectMapRef["Attack"] = Instantiate(effectMap["Attack"].prefab, attackSocket.transform.position, Quaternion.identity);
            Animator animator = effectMapRef["Attack"].GetComponent<Animator>();
            animator.SetTrigger("Effect");
        }            
    }


    void EffectAttackImpact()
    {
        if (effectMapRef["Attack"])
        {
            Destroy(effectMapRef["Attack"]);
        }        

        Debug.Log("AttackImpact");
        //animator1.SetBool("Effect", false);파괴하는데..?
        effectMapRef["AttackImpact"] = Instantiate(effectMap["AttackImpact"].prefab, skillEndPos.transform.position, Quaternion.identity);
        Animator animator = effectMapRef["AttackImpact"].GetComponent<Animator>();
        animator.SetTrigger("Effect");
    }

    public void EffectShield()
    {
        Vector3 offset2 = new Vector3(0, 0, -20);
        effectMapRef["Shield"] = Instantiate(effectMap["Shield"].prefab, shieldSocket.transform.position, Quaternion.identity);
        Animator animator = effectMapRef["Shield"].GetComponent<Animator>();
        animator.SetTrigger("Effect");

    }

    public void EffectHeal()
    {
        Vector3 offset2 = new Vector3(0, 0, -20);
        effectMapRef["Heal"] = Instantiate(effectMap["Heal"].prefab, healSocket.transform.position, Quaternion.identity);
        Animator animator = effectMapRef["Heal"].GetComponent<Animator>();
        animator.SetTrigger("Effect");
    }
    public void EffectDefense()
    {
        Vector3 offset2 = new Vector3(0, 0, -20);
        effectMapRef["Defense"] = Instantiate(effectMap["Defense"].prefab, defenseSocket.transform.position, Quaternion.identity);
        Animator animator = effectMapRef["Defense"].GetComponent<Animator>();
        animator.SetTrigger("Effect");
    }

    public void EndEffectAttack()
    {
        Destroy(effectMapRef["Attack"]);
    }

    public void EndEffectAttackImpact()
    {
        Destroy(effectMapRef["AttackImpact"]);
    }

    public void EndEffectShield()
    {
        Destroy(effectMapRef["Shield"]);
    }

    public void EndEffectHeal()
    {
        Destroy(effectMapRef["Heal"]);
    }

    public void EndEffectDefense()
    {
        Destroy(effectMapRef["Defense"]);
    }
    /*
    public void EndAttack1Hurted()
    {
        Destroy(Attack1HurtedEffect);
    }*/
}
