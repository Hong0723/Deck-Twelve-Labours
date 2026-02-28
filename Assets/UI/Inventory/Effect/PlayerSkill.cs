using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public List<EffectData> effects;//인스펙터 등록용
    private Dictionary<string, EffectObjBol> effectMap;//코드내 서치용, 원본 오브젝트
    public List<EffectObjBol> AttackEffects;//3가지 공격모션 저장용    
    private Dictionary<string, GameObject> effectMapRef;//생성된 인스턴스 관리용
    public GameObject skillStartPos;//스킬이펙트 시작점
    public GameObject skillEndPos;//스킬이펙트 끝나는 지점
    private bool isMoveAble;//스킬이펙트가 이동하면서 실행되어야 할때
    public float speed;//이동속도
    

    void Start()
    {
        isMoveAble = false;
        speed = 5f;
        effectMap = new Dictionary<string, EffectObjBol>();
        effectMapRef = new Dictionary<string, GameObject>();

        //이름에 Attack이 들어가있다면 공격리스트에 저장(추후 랜덤 뽑기를 위해)
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
    }
    
    public void EffectAttack()
    {
        //이전에 사용하던 이펙트가 안지워졌을 경우 한번더 지우는 코드
        if (effectMapRef.ContainsKey("Attack") && effectMapRef["Attack"] != null)
        {
            Destroy(effectMapRef["Attack"]);
        }
        
        //랜덤 공격모션
        int AttackIndex = Random.Range(0, AttackEffects.Count);
        
        if (false)//스킬이펙트가 이동하면서 실행되어야 할때
        {           
            isMoveAble = true;
            effectMapRef["Attack"] = Instantiate(AttackEffects[AttackIndex].prefab, skillStartPos.transform.position, Quaternion.identity);
            Animator animator = effectMapRef["Attack"].GetComponent<Animator>();
            animator.SetTrigger("Effect");            
        }
        else//제자리 이펙트 재생
        {           
            //생성된 이펙트 인스턴스 저장
            effectMapRef["Attack"] = Instantiate(AttackEffects[AttackIndex].prefab, skillStartPos.transform.position, Quaternion.identity);
            Animator animator = effectMapRef["Attack"].GetComponent<Animator>();
            animator.SetTrigger("Effect");
        }

        EffectAttackImpact();//피격이펙트
    }

    public void EffectAttackImpact()
    {
        if (effectMapRef.ContainsKey("HurtedImpact") && effectMapRef["HurtedImpact"] != null)
        {
            Destroy(effectMapRef["HurtedImpact"]);
        }                
        effectMapRef["HurtedImpact"] = Instantiate(effectMap["HurtedImpact"].prefab, skillEndPos.transform.position, Quaternion.identity);
        Animator animator = effectMapRef["HurtedImpact"].GetComponent<Animator>();
        animator.SetTrigger("Effect");
    }

    public void EffectHeal()
    {
        if (effectMapRef.ContainsKey("Heal") && effectMapRef["Heal"] != null)
        {
            Destroy(effectMapRef["Heal"]);
        }           
        effectMapRef["Heal"] = Instantiate(effectMap["Heal"].prefab, skillStartPos.transform.position, Quaternion.identity);
        Animator animator = effectMapRef["Heal"].GetComponent<Animator>();
        animator.SetTrigger("Effect");
    }

    public void EffectShield()
    {
        if (effectMapRef.ContainsKey("Shield") && effectMapRef["Shield"] != null)
        {
            Destroy(effectMapRef["Shield"]);
        }
        effectMapRef["Shield"] = Instantiate(effectMap["Shield"].prefab, skillStartPos.transform.position, Quaternion.identity);
        Animator animator = effectMapRef["Shield"].GetComponent<Animator>();
        animator.SetTrigger("Effect");
    }

    public void EffectDraw()
    {
        if (effectMapRef.ContainsKey("Draw") && effectMapRef["Draw"] != null)
        {
            Destroy(effectMapRef["Draw"]);
        }
        effectMapRef["Draw"] = Instantiate(effectMap["Draw"].prefab, skillStartPos.transform.position, Quaternion.identity);
        Animator animator = effectMapRef["Draw"].GetComponent<Animator>();
        animator.SetTrigger("Effect");
    }

    //이펙트 애니메이션 끝에 AddAnimationEvent로 호출하여 사용된 인스턴스 제거
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
