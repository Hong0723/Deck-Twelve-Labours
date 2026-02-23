using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public List<EffectData> effects;//인스펙터 등록용
    private Dictionary<string, EffectObjBol> effectMap;//코드내 서치용, 원본 오브젝트
    public List<EffectObjBol> AttackEffects;//3가지 공격
    //private Dictionary<string, EffectObjBol> effectMapRef;//Instantiate 후 여기에 저장,후에는 Destroy관리
    private Dictionary<string, GameObject> effectMapRef; // 생성된 인스턴스

    public GameObject skillStartPos;//스킬 시작점
    public GameObject skillEndPos;//플레이어가 스킬 맞는 지점
    private bool isMoveAble;
    public float speed;
/*
    // ===== 싱글톤 =====
    public static PlayerSkill Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 넘어가도 유지하려면 사용
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // ==================
    */
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isMoveAble = false;
        speed = 5f;
        effectMap = new Dictionary<string, EffectObjBol>();
        effectMapRef = new Dictionary<string, GameObject>();
        foreach (var e in effects)
        {
            if (e.skillNameHaveImpact.StartsWith("Attack"))
            {
                AttackEffects.Add(e.data);
            }
            else if (!effectMap.ContainsKey(e.skillNameHaveImpact))
            {
                effectMap.Add(e.skillNameHaveImpact, e.data);
            }                
        }        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoveAble)
        {
            effectMapRef["Attack"].transform.position = Vector3.Lerp(effectMapRef["Attack"].transform.position, skillEndPos.transform.position, Time.deltaTime * speed);

            if (Vector3.Distance(effectMapRef["Attack"].transform.position, skillEndPos.transform.position) < 0.5f)
            {
                EffectAttackImpact();
                isMoveAble = false;
            }
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            EffectAttack();
        }
    }
    /*
    public static void EffAttack()
    {
        Instance.EffectAttack();
    }
    public static void EffDraw()
    {
        Instance.EffectDraw();
    }
    public static void EffHeal()
    {
        Instance.EffectHeal();
    }
    public static void EffShield()
    {
        Instance.EffectShield();
    }
    */
    public void EffectAttack()
    {
        int AttackIndex = Random.Range(0, AttackEffects.Count);
        
        if (false)//(effectMap["Attack"].haveImpaact)
        {//플레이어가 스킬에 대한 피격애니메이션이 있을때
            Debug.Log("Attack 생성");
            //effectMapRef["Attack"] = Instantiate(effectMap["Attack"].prefab, skillStartPos.transform.position, Quaternion.identity);
            effectMapRef["Attack"] = Instantiate(AttackEffects[AttackIndex].prefab, skillStartPos.transform.position, Quaternion.identity);
            Animator animator = effectMapRef["Attack"].GetComponent<Animator>();
            animator.SetTrigger("Effect");            
        }
        else
        {//없을때
            //effectMapRef["Attack"] = Instantiate(effectMap["Attack"].prefab, skillStartPos.transform.position, Quaternion.identity);
            effectMapRef["Attack"] = Instantiate(AttackEffects[AttackIndex].prefab, skillStartPos.transform.position, Quaternion.identity);
            Animator animator = effectMapRef["Attack"].GetComponent<Animator>();
            animator.SetTrigger("Effect");
        }

        EffectAttackImpact();//때리면 피격모션까지
    }

    public void EffectAttackImpact()
    {       
        Debug.Log("HurtedImpact");
        //animator1.SetBool("Effect", false);파괴하는데..?
        effectMapRef["HurtedImpact"] = Instantiate(effectMap["HurtedImpact"].prefab, skillEndPos.transform.position, Quaternion.identity);
        Animator animator = effectMapRef["HurtedImpact"].GetComponent<Animator>();
        animator.SetTrigger("Effect");
    }

    public void EffectHeal()
    {
        Debug.Log("Heal");
        //animator1.SetBool("Effect", false);파괴하는데..?
        effectMapRef["Heal"] = Instantiate(effectMap["Heal"].prefab, skillStartPos.transform.position, Quaternion.identity);
        Animator animator = effectMapRef["Heal"].GetComponent<Animator>();
        animator.SetTrigger("Effect");
    }

    public void EffectShield()
    {
        Debug.Log("Shield");
        //animator1.SetBool("Effect", false);파괴하는데..?
        effectMapRef["Shield"] = Instantiate(effectMap["Shield"].prefab, skillStartPos.transform.position, Quaternion.identity);
        Animator animator = effectMapRef["Shield"].GetComponent<Animator>();
        animator.SetTrigger("Effect");
    }

    public void EffectDraw()
    {
        Debug.Log("Draw");
        //animator1.SetBool("Effect", false);파괴하는데..?
        effectMapRef["Draw"] = Instantiate(effectMap["Draw"].prefab, skillStartPos.transform.position, Quaternion.identity);
        Animator animator = effectMapRef["Draw"].GetComponent<Animator>();
        animator.SetTrigger("Effect");
    }
    public void EndEffectAttack()
    {
        Destroy(effectMapRef["Attack"]);
    }

    public void EndEffectAttackImpact()
    {
        Destroy(effectMapRef["HurtedImpact"]);
    }

    public void EndEffectHealImpact()
    {
        Destroy(effectMapRef["Heal"]);
    }

    public void EndEffectShieldImpact()
    {
        Destroy(effectMapRef["Shield"]);
    }

    public void EndEffectDrawImpact()
    {
        Destroy(effectMapRef["Draw"]);
    }


}
