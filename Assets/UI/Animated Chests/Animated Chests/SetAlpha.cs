using UnityEngine;
using UnityEngine.UI;

public class SetAlpha : MonoBehaviour
{
    public void SetAlpha1()
    {
        Image img = GetComponent<Image>();

        Color color = img.color;
        color.a = 1f;   // 0 = 완전투명, 1 = 완전불투명
        img.color = color;
    }

    public void SetAlpha0()
    {
        Image img = GetComponent<Image>();

        Color color = img.color;
        color.a = 0;   // 0 = 완전투명, 1 = 완전불투명
        img.color = color;
    }
}
