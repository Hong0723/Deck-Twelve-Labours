using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHP = 30;
    public int currentHP;
    public int block;

    public HPBar hpBar;

    public Animator animator;  
    private Coroutine attackCoroutine;//실행중인 공격애니메이션

    void Start()
    {
        
        currentHP = maxHP;
        block = 0;

        //스크립터블 오브젝트에서 플레이어 정보를 가져옵니다
        if (DeliverBattleData.PlayerInfo)
        {
            MonsterType playerInfo = DeliverBattleData.PlayerInfo;
            maxHP = playerInfo.maxHP;
            currentHP = maxHP;
            block = playerInfo.block;        
        }
        

        UpdateHPBar();        
    }

    public void TakeDamage(int dmg)
    {
        int absorbed = Mathf.Min(block, dmg);
        block -= absorbed;
        dmg -= absorbed;

        currentHP = Mathf.Max(currentHP - dmg, 0);
        UpdateHPBar();
        

    }

    public void Heal(int amount)
    {
        currentHP = Mathf.Min(maxHP, currentHP + amount);
        UpdateHPBar();
    }

    void UpdateHPBar()
    {
        if (hpBar)
            hpBar.Set(currentHP, maxHP, block);
    }

    public bool IsDead() => currentHP <= 0;


    public void Attack1Animation()
    {
        //animator.SetTrigger("Attack1");
        attackCoroutine = StartCoroutine(Attack1Coroutine());
    }

    public void Attack2Animation()
    {

        //animator.SetTrigger("Attack2");
        attackCoroutine = StartCoroutine(Attack2Coroutine());
    }
    public void HurtedAnimation()
    {        
        StartCoroutine(HurtedCoroutine());
    }


    public void SetDefensed(bool state)
    {
        animator.SetBool("Defensed", state);
    }

    // 공격 1
    public IEnumerator Attack1Coroutine()
    {
        animator.SetTrigger("Attack1");

        // Attack1 애니메이션 길이만큼 대기
        yield return new WaitForSeconds(GetAnimationLength("PlayerAttack1"));

        // 애니 끝나면 Idle로 자동 전환 (Animator Exit Time 사용)
    }

    // 공격 2
    public IEnumerator Attack2Coroutine()
    {
        animator.SetTrigger("Attack2");
        yield return new WaitForSeconds(GetAnimationLength("PlayerAttack2"));
    }

    // 피격
    public IEnumerator HurtedCoroutine()
    {
        animator.SetTrigger("Hurted");
        yield return new WaitForSeconds(GetAnimationLength("Damaged"));
    }

    // Animation Clip 길이 가져오기
    private float GetAnimationLength(string clipName)
    {
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
                return clip.length;
        }
        Debug.LogWarning("Clip not found: " + clipName);
        return 0f;
    }
    /*
    public void resetAttackTrigger()
    {
        //공격 Trigger 제거
        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
    }

    public void resetHurtedTrigger()
    {
        //공격 Trigger 제거
        animator.ResetTrigger("Hurted");
        
    }*/
}

