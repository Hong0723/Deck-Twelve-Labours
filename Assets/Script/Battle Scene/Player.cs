using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    //수치 확인용
    public int maxHP = 30;
    public int currentHP;
    public int block;
    public int attackDamge;//공격력
                          
    public ItemUseManager hpBar;//Hpbar를 상속 받는 아이템 사용시 실행되는 스크립트
    public Animator animator;//플레이어 애니메이션
    public PlayerSkill playerSkill;//이펙트 애니메이션은 여기서 실행

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
        HurtedAnimation();
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
        playerSkill.EffectHeal();        
        amount += DeliverBattleData.PlayerInfo.Heal;//아이템으로 인한 추가회복량        
        GlobalPlayerHP.Heal(amount);

        SyncFromGlobal();
        UpdateHPBar();
    }

    public void GainShield(int amount)
    {
        playerSkill.EffectShield();        
        amount+= DeliverBattleData.PlayerInfo.shield;//아이템으로 인한 추가쉴드량        
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
        animator.SetTrigger("Attack1");       
    }    

    public void HurtedAnimation()
    {
        animator.SetTrigger("Hurted");        
    }

    public void SetDefensed(bool state)
    {
        animator.SetBool("Defensed", state);
    }   
}
