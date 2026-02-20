using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHP = 30;
    public int currentHP;
    public int block;
    public int attackDamge;//공격력

    //public HPBar hpBar;
    public ItemUseManager hpBar;

    public Animator animator;
    private Coroutine attackCoroutine; // 실행중인 공격애니메이션

    private void Start()
    {
        // 기본 block은 배틀 진입 시점에만 초기화(HP는 전역 유지)
        block = 0;

        // 스크립터블 오브젝트에서 플레이어 정보를 가져옵니다 (최초 초기화/MaxHP 갱신)
        if (DeliverBattleData.PlayerInfo)
        {
            MonsterType playerInfo = DeliverBattleData.PlayerInfo;
            GlobalPlayerHP.InitializeIfNeeded(playerInfo.maxHP);
            GlobalPlayerHP.UpdateMaxHP(playerInfo.maxHP);

            block = playerInfo.block;
            attackDamge = playerInfo.AttackDamage;
        }
        else
        {
            // PlayerInfo가 없더라도, 인스펙터 maxHP로 전역 초기화는 보장
            GlobalPlayerHP.InitializeIfNeeded(maxHP);
            GlobalPlayerHP.UpdateMaxHP(maxHP);
        }

        // 전역 HP를 로컬 미러링(기존 UI/기존 코드 호환)
        SyncFromGlobal();
        UpdateHPBar();
    }

    
    public void TakeDamage(int dmg)
    {       
        int absorbed = Mathf.Min(block, dmg);
        block -= absorbed;
        dmg -= absorbed;
                
        GlobalPlayerHP.Damage(dmg);

        //게임 오버 창 실행하는 코드가 PlayerStatus.cs부분에 존재하여 
        //중복 로직 실행..
        GetComponent<PlayerStatus>().TakeDamage(dmg);

        SyncFromGlobal();
        UpdateHPBar();        
    }

    public void Heal(int amount)
    {
        Debug.Log("힐량" + amount);
        amount += DeliverBattleData.PlayerInfo.Heal;//아이템으로 인한 추가회복량
        Debug.Log("힐량" + amount);
        GlobalPlayerHP.Heal(amount);

        SyncFromGlobal();
        UpdateHPBar();
    }

    public void GainShield(int amount)
    {
        Debug.Log("쉴드량" + amount);
        amount+= DeliverBattleData.PlayerInfo.shield;//아이템으로 인한 추가쉴드량
        Debug.Log("쉴드량" + amount);
        if (amount <= 0) return;

        block += amount;
        UpdateHPBar();
    }

    private void SyncFromGlobal()
    {
        maxHP = GlobalPlayerHP.MaxHP;
        currentHP = GlobalPlayerHP.CurrentHP;
    }

    private void UpdateHPBar()
    {
        if (hpBar)
            hpBar.Set(currentHP, maxHP, block);
    }

    public bool IsDead() => currentHP <= 0;

    public void Attack1Animation()
    {
        attackCoroutine = StartCoroutine(Attack1Coroutine());
    }

    public void Attack2Animation()
    {
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
}
