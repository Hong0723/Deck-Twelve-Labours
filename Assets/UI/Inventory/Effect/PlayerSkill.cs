using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public List<EffectData> effects;//인스펙터 등록용
    private Dictionary<string, EffectObjBol> effectMap;//코드내 서치용, 원본 오브젝트
    //private Dictionary<string, EffectObjBol> effectMapRef;//Instantiate 후 여기에 저장,후에는 Destroy관리
    private Dictionary<string, GameObject> effectMapRef; // 생성된 인스턴스

    public GameObject skillStartPos;//스킬 시작점
    public GameObject skillEndPos;//플레이어가 스킬 맞는 지점
    private bool isMoveAble;
    public float speed;
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
    }

    public void EffectAttack()
    {
        if (false)//(effectMap["Attack"].haveImpaact)
        {//플레이어가 스킬에 대한 피격애니메이션이 있을때
            Debug.Log("Attack 생성");
            effectMapRef["Attack"] = Instantiate(effectMap["Attack"].prefab, skillStartPos.transform.position, Quaternion.identity);
            Animator animator = effectMapRef["Attack"].GetComponent<Animator>();
            animator.SetTrigger("Effect");            
        }
        else
        {//없을때
            effectMapRef["Attack"] = Instantiate(effectMap["Attack"].prefab, skillStartPos.transform.position, Quaternion.identity);
            Animator animator = effectMapRef["Attack"].GetComponent<Animator>();
            animator.SetTrigger("Effect");
        }

        EffectAttackImpact();//때리면 피격모션까지
    }

    void EffectAttackImpact()
    {       
        Debug.Log("AttackImpact");
        //animator1.SetBool("Effect", false);파괴하는데..?
        effectMapRef["AttackImpact"] = Instantiate(effectMap["AttackImpact"].prefab, skillEndPos.transform.position, Quaternion.identity);
        Animator animator = effectMapRef["AttackImpact"].GetComponent<Animator>();
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
}
