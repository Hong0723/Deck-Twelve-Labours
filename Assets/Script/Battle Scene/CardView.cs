using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class CardView : MonoBehaviour
{
    [SerializeField] private TMP_Text title;

    [SerializeField] private TMP_Text description;

    [SerializeField] private TMP_Text mana;

    [SerializeField] private  SpriteRenderer imageSR;

    [SerializeField] private GameObject wrapper;
     
    [SerializeField] private LayerMask dropLayer;

    public Card Card {get; private set;}

    private Vector3 dragStartPosition;

    private Quaternion dragStartRotation;

    public void Setup(Card card)
    {
        Card = card;
        title.text = card.Title;
        description.text = card.Description;
        mana.text = card.Mana.ToString();
        imageSR.sprite = card.Image;
    }

    void OnMouseEnter()
    {
        if (!Interactions.Instance.PlayerCanHover()) return;
        wrapper.SetActive(false);
        Vector3 pos = new(0, 0, 0);
        CardViewHoverSystem.Instance.Show(Card, pos);
    }

    void OnMouseExit()
    {
        if (!Interactions.Instance.PlayerCanHover()) return;
        CardViewHoverSystem.Instance.Hide();
        wrapper.SetActive(true);
    }
    void OnMouseDown()
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;
        if (Card.ManualTargetEffect != null)
        {
            ManualTargetSystem.Instance.StartTargeting(transform.position);
        }
        else
        {
            Interactions.Instance.PlayerIsDragging = true;
            wrapper.SetActive(true);
            CardViewHoverSystem.Instance.Hide();
            dragStartPosition = transform.position;
            dragStartRotation = transform.rotation;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            Vector3 mousePos = MouseUtil.GetMousePositionInWorldSpace(dragStartPosition.z); // ���� ���� ����
            transform.position = mousePos;
        }
        
    }


    void OnMouseDrag()
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;
        if (Card.ManualTargetEffect != null) return;
        transform.position = MouseUtil.GetMousePositionInWorldSpace(dragStartPosition.z);
    }

    void OnMouseUp()
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;
        if (Card.ManualTargetEffect != null)
        {
            Vector3 mouseWorldPos = MouseUtil.GetMousePositionInWorldSpace(100f); 
        
            EnemyStatus target = ManualTargetSystem.Instance.EndTargeting(mouseWorldPos);
            if (target != null && ManaSystem.Instance.HasEnoughMana(Card.Mana))
            {
                Debug.Log($"공격 시도: {target.name}에게 {Card.Title} 발사!");
                PlayCardGA playCardGA = new(Card, target);
                ActionSystem.Instance.Perform(playCardGA);
            }
        }
        else
        {
            if (ManaSystem.Instance.HasEnoughMana(Card.Mana)
                && Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, 10f, dropLayer))
            {
                PlayCardGA playCardGA = new(Card);
                ActionSystem.Instance.Perform(playCardGA);
            }
            else
            {
                transform.position = dragStartPosition;
                transform.rotation = dragStartRotation;
            }
            Interactions.Instance.PlayerIsDragging = false;
        }
        

    }
}

