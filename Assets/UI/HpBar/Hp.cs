using UnityEngine;
using UnityEngine.UI;

public class Hp : MonoBehaviour
{
    [SerializeField]
    private float curHealth;
    [SerializeField]
    private float maxHealth;
    public Slider HpBarSlider;
    //체력은 0~1 실수범위
    //여기서는 빨간색이 curHealth값에 따라 빨간색 바의 비율 조절하는 정도만 구현되어있습니다.
    void Start()
    {
        curHealth = 100f;
        maxHealth = 100f;

        if (HpBarSlider != null)
            HpBarSlider.value = curHealth / maxHealth;
    }

    public void CheckHp() //*HP 갱신
    {
        if (HpBarSlider != null)
            HpBarSlider.value = curHealth / maxHealth;
    }

    public void Damage(float damage) //* 데미지 받는 함수
    {
        if (maxHealth == 0 || curHealth <= 0) //* 이미 체력 0이하면 패스
            return;
        curHealth -= damage;
        CheckHp(); //* 체력 갱신
        if (curHealth <= 0)
        {
            //* 체력이 0 이하라 죽음
        }
    }

    public void SetCurHp(float amount)
    {
        curHealth = amount;
    }
    public void SetMaxHp(float amount)
    {
        maxHealth = amount;
    }
}
