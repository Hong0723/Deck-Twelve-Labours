using System.Collections;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    [SerializeField] private GameObject damageVFX;
    [SerializeField] private Player player;
    [SerializeField] private PlayerSkill playerSkill;
    void OnEnable()
    {
        ActionSystem.AttachPerformer<DealDamageGA>(DealDamagePerformer);
    }
    void OnDisable()
    {
        ActionSystem.DetachPerformer<DealDamageGA>();
    }
    private IEnumerator DealDamagePerformer(DealDamageGA dealDamageGA)
    {
        player.Attack1Animation();//플레이어 애니메이션
        playerSkill.EffectAttack();//공격이펙트
        foreach (var target in dealDamageGA.Targets)
        {
            // 대상이 적이든 플레이어든 IDamageable을 가지고 있으면 작동            
            target.TakeDamage(dealDamageGA.Amount);
            // 연출 로직
            if (target is MonoBehaviour mono)
            {
                if (damageVFX != null)
                    Instantiate(damageVFX, mono.transform.position, Quaternion.identity);             
            }
            yield return new WaitForSeconds(0.15f);
        }
    }
}

