using UnityEngine;

public class HPBar : MonoBehaviour
{
    public RectTransform hpFill;
    public RectTransform shieldFill;

    RectTransform barRect;
    float barWidth;

    void Awake()
    {
        barRect = GetComponent<RectTransform>();
        barWidth = barRect.rect.width;
    }

    public void Set(int currentHP, int maxHP, int shield)
    {
        float hpRatio = Mathf.Clamp01((float)currentHP / maxHP);
        float shieldRatio = Mathf.Clamp01((float)shield / maxHP);

        float hpWidth = barWidth * hpRatio;
        hpFill.sizeDelta = new Vector2(hpWidth, hpFill.sizeDelta.y);

        float shWidth = barWidth * shieldRatio;
        shieldFill.sizeDelta = new Vector2(shWidth, shieldFill.sizeDelta.y);

        shieldFill.anchoredPosition = new Vector2(hpWidth, 0);
        shieldFill.gameObject.SetActive(shield > 0);
    }
}
