using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Portion : MonoBehaviour, IPointerClickHandler
{
    public int portionCount;
    public Sprite usePortionImage;

    public void OnPointerClick(PointerEventData eventData)
    {
        portionCount--;
        if (portionCount <= 0)
        {
            Image portionImage = GetComponent<Image>();
            portionImage.sprite = usePortionImage;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        portionCount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
