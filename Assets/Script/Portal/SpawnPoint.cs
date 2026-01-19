using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private string spawnId = "Default";
    public string SpawnId => spawnId;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }
#endif
}
