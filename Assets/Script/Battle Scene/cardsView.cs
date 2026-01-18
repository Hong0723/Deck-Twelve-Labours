using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class cardsView : MonoBehaviour
{
    [SerializeField] private TMP_Text title;

    [SerializeField] private TMP_Text description;

    [SerializeField] private TMP_Text mana;

    [SerializeField] private  SpriteRenderer imageSR;

    [SerializeField] private GameObject wrapper;

    public Card Card {get; private set;}

    public void Setup(Card card)
    {
        Card = card;
        title.text = card.Title;
        description.text = card.Description;
        mana.text = card.Mana.ToString();
        imageSR.sprite = card.Image;
    }
}
