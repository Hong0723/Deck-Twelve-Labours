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

    // 🔥 핵심: 시작점을 적(90)보다 훨씬 앞인 카메라 근처(-10)로 옮깁니다.
    // 마우스의 X, Y 좌표는 유지하되 Z값만 카메라 쪽으로 당깁니다.
    Vector3 rayStartPos = new Vector3(endPosition.x, endPosition.y, -10f); 

    // 이제 -10에서 90을 향해(Vector3.forward) 쏘기 때문에 무조건 적을 뚫고 지나갑니다.
    if (Physics.Raycast(rayStartPos, Vector3.forward, out RaycastHit hit, 1000f, targetLayerMask))
    {
        if (hit.transform.TryGetComponent(out EnemyStatus enemyStatus))
        {
            return enemyStatus;
        }
    }
    else
    {
        Debug.Log($"공중에 쏨. 시작점:{rayStartPos}, 레이어:{LayerMask.LayerToName(targetLayerMask)}");
    }   
    return null;
}
}
