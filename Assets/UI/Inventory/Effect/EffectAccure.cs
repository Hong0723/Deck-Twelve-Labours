using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class EffectAccure : MonoBehaviour
{
    public GameObject player;
    
    private Animator animator1;
    private Animator animator2;
    GameObject BrashEffect;
    GameObject ExplodeEffect;
    GameObject ShieldEffect;
    GameObject HealEffect;
    GameObject DefenseEffect;

    public float speed = 5f;
    public GameObject brashPrefab;
    public GameObject hitPrefab;
    public GameObject shieldPrefab;
    public GameObject healPrefab;
    public GameObject defensePrefab;
    private bool isMoveAble;
    private Vector3 offset = new Vector3(0, 15, 20);
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isMoveAble = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoveAble)
        {
            BrashEffect.transform.position = Vector3.Lerp(BrashEffect.transform.position, player.transform.position + offset, Time.deltaTime * speed);

            if(Vector3.Distance(BrashEffect.transform.position, player.transform.position + offset) < 0.5f)
            {
                ActiveExplode();
                isMoveAble = false;
            }
        }
        

    }

    public void ActiveBrash()
    {
        Debug.Log("Brash »ý¼º");
        BrashEffect = Instantiate(brashPrefab, transform.position, Quaternion.identity);
        animator1 = BrashEffect.GetComponent<Animator>();
        animator1.SetBool("Effect", true);
        isMoveAble = true;
    }

    void ActiveExplode()
    {
        Destroy(BrashEffect);

        Debug.Log("Æø¹ß");
        animator1.SetBool("Effect", false);
        ExplodeEffect = Instantiate(hitPrefab, player.transform.position + offset, Quaternion.identity);
        animator2 = ExplodeEffect.GetComponent<Animator>();
        animator2.SetTrigger("Effect");

        
        
    }

    public void ActiveShield()
    {
        Vector3 offset2 = new Vector3(0, 0, -20);
        ShieldEffect = Instantiate(shieldPrefab, transform.position+offset2, Quaternion.identity);
        Animator animator = ShieldEffect.GetComponent<Animator>();
        animator.SetTrigger("Effect");
        
    }

    public void ActiveHeal()
    {
        Vector3 offset2 = new Vector3(0, 0, -20);
        HealEffect = Instantiate(healPrefab, transform.position + offset2, Quaternion.identity);
        Animator animator = HealEffect.GetComponent<Animator>();
        animator.SetTrigger("Effect");
    }
    public void ActiveDefense()
    {
        Vector3 offset2 = new Vector3(0, 0, -20);
        DefenseEffect = Instantiate(defensePrefab, transform.position + offset2, Quaternion.identity);
        Animator animator = DefenseEffect.GetComponent<Animator>();
        animator.SetTrigger("Effect");
    }

    public void EndExplode()
    {
        Destroy(ExplodeEffect);
    }

    public void EndShield()
    {
        Destroy(ShieldEffect);
    }

    public void EndHeal()
    {
        Destroy(HealEffect);
    }

    public void EndDefense()
    {
        Destroy(DefenseEffect);
    }
}
