using UnityEngine;

public class AlarmEndEffect : MonoBehaviour
{

    [SerializeField]
    private GameObject effectAccureObj;
    private EffectAccure effectAccureScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        effectAccureScript = effectAccureObj.GetComponent<EffectAccure>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndExplode()
    {
        
        effectAccureScript.EndExplode();
    }

    public void EndShield()
    {
        
        effectAccureScript.EndShield();
    }

    public void EndHeal()
    {
        effectAccureScript.EndHeal();
    }

    public void EndDefense()
    {
        effectAccureScript.EndDefense();
    }
}
