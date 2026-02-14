using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class HPBar : MonoBehaviour
{
    public RectTransform hpFill;
    public RectTransform shieldFill;
    public RectTransform TrailFill;//잔상효과
    
    bool updateBar;
    float TrailRatio;
    

    RectTransform barRect;
    float barWidth;


    protected void Awake()
    {
        barRect = GetComponent<RectTransform>();
        barWidth = barRect.rect.width;
        updateBar = false;
        
        hpFill = transform.Find("Hp Fill").GetComponentInChildren<RectTransform>();
        shieldFill = transform.Find("Shield Fill").GetComponentInChildren<RectTransform>();
        TrailFill = transform.Find("Trail Fill").GetComponentInChildren<RectTransform>();
        
        Debug.Log("HPBar on: " + gameObject.name);
    }
    void Update()
    {
        
        if (!updateBar) return;

        float trailWidth = barWidth * TrailRatio;

        Vector2 size = TrailFill.sizeDelta;
        size.x = Mathf.Lerp(size.x, trailWidth, Time.deltaTime * 2f);
        TrailFill.sizeDelta = size;

        //목표값에 도달했는지 체크
        if (Mathf.Abs(size.x - trailWidth) < 0.1f) 
        {
            size.x = trailWidth;          
            TrailFill.sizeDelta = size;
            updateBar = false;
        }
    }

    public void Set(int currentHP, int maxHP, int shield)
    {
        float hpRatio = Mathf.Clamp01((float)currentHP / maxHP);
        float shieldRatio = Mathf.Clamp01((float)shield / maxHP);
        TrailRatio = Mathf.Clamp01((float)currentHP / maxHP);
        //Debug.Log("hpRatio: " + hpRatio);
        float hpWidth = barWidth * hpRatio;
        if (hpFill == null)
        {
            Debug.Log("hpfill null");
            return;
        }
        hpFill.sizeDelta = new Vector2(hpWidth, hpFill.sizeDelta.y);

        float shWidth = barWidth * shieldRatio;
        if (shieldFill == null)
        {
            Debug.Log("shieldfill null");
            return;
        }
        shieldFill.sizeDelta = new Vector2(shWidth, shieldFill.sizeDelta.y);
        shieldFill.anchoredPosition = new Vector2(hpWidth, 0);
        shieldFill.gameObject.SetActive(shield > 0);

        updateBar = true;
    }
}
