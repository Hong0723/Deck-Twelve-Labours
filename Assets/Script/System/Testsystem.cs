using UnityEngine;

public class Testsystem : MonoBehaviour
{
    [SerializeField] private HandView handView;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            cardsView cardView = CardViewCreator.Instance.CreateCardView(transform.position, Quaternion.identity);
            StartCoroutine(handView.AddCard(cardView));  
        }
    }
}
