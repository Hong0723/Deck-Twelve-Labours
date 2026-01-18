using UnityEngine;

public class Testsystem : MonoBehaviour
{
    [SerializeField] private HandView handView;

    [SerializeField] private CardData2 cardData;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Card card = new(cardData);
            cardsView cardView = CardViewCreator.Instance.CreateCardView(card, transform.position, Quaternion.identity);
            StartCoroutine(handView.AddCard(cardView));  
        }
    }
}
