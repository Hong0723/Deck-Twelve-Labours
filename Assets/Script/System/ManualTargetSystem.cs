using UnityEngine;

public class ManualTargetSystem : Singleton<ManualTargetSystem>
{
    [SerializeField] private ArrowView arrowView;

    [SerializeField] private LayerMask targetLayerMask;

    public void StartTargeting(Vector3 startPosition)
    {
        arrowView.gameObject.SetActive(true);
        arrowView.SetupArrow(startPosition);
    }
    
    public EnemyStatus EndTargeting(Vector3 endPosition)
    {
        arrowView.gameObject.SetActive(false);

        // 레이캐스트로 마우스 위치에 적이 있는지 확인
        if (Physics.Raycast(endPosition, Vector3.forward, out RaycastHit hit, 10f, targetLayerMask)
            && hit.collider != null
            && hit.transform.TryGetComponent(out EnemyStatus enemyStatus))
        {
            return enemyStatus; // 적을 찾았다면 해당 적의 Status 반환
        }
        return null;
    }
}
