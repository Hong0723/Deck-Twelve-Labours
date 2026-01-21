using UnityEngine;

public class PlayerEffectAccure : MonoBehaviour
{

    GameObject Attack1Effect;
    GameObject Attack2Effect;
    GameObject ShieldEffect;
    GameObject HurtedEffect;


    public GameObject Attack1Prefab;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActiveAttack1()
    {
        Vector3 offset2 = new Vector3(0, 0, -20);
        Attack1Effect = Instantiate(Attack1Prefab, transform.position + offset2, Quaternion.identity);
        Animator animator = Attack1Effect.GetComponent<Animator>();
        animator.SetTrigger("Effect");
    }

    public void EndAttack1()
    {
        Destroy(Attack1Effect);
    }
}
