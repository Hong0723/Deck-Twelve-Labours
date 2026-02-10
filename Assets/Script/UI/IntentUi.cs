using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class IntentUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Enemy enemy;
    public Image iconImage;
    public GameObject tooltipObject;
    public TextMeshProUGUI tooltipText;

    [Header("Action Sprites")]
    public Sprite attackSprite;
    public Sprite shieldSprite;
    public Sprite healSprite;
    public Sprite defenseSprite;
    void Start()
    {
        UpdateIntent();
    }

    public void UpdateIntent()
    {
        if (enemy == null) return;

        tooltipText.text = enemy.GetIntentDescription();
         switch (enemy.nextAction)
        {
            case EnemyActionType.Attack:
                iconImage.sprite = attackSprite;
                break;

            case EnemyActionType.Shield:
                iconImage.sprite = shieldSprite;
                break;

            case EnemyActionType.Heal:
                iconImage.sprite = healSprite;
                break;

            case EnemyActionType.Defense:
                iconImage.sprite = defenseSprite;
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipObject.SetActive(false);
    }
    public void SetEnemy(Enemy target)
{
    enemy = target;
    UpdateIntent();
}
}