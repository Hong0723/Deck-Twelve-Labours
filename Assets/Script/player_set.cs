using UnityEngine;

public class player_set : MonoBehaviour
{
    public float moveSpeed = 5f; // 이동 속도 (Inspector에서 조절 가능)

    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 movement;
    
    // 마지막으로 입력한 방향을 저장할 변수 (Idle 상태를 위해)
    private Vector2 lastMoveDirection;

    //GameScene에서 BattleScene으로 넘어갈때 필요한 현재 플레이어 정보(스크립터블 오브젝트)
    [SerializeField] private MonsterType PlayerInfo;
    void Start()
    {
        // 필요한 컴포넌트들을 미리 가져옵니다.
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        // 중요: 시작할 때 기본 방향을 '아래'로 설정해줍니다.
        // 이렇게 해야 'player_stand_down' 상태로 시작합니다.
        lastMoveDirection = new Vector2(0, -1); 
        anim.SetFloat("moveX", 0);
        anim.SetFloat("moveY", -1);

        //static에 값 저장
        DeliverBattleData.PlayerInfo = PlayerInfo;

        // 전역 HP 최초 초기화(씬 이동 시에도 유지)
        if (PlayerInfo != null)
        {
            GlobalPlayerHP.InitializeIfNeeded(PlayerInfo.maxHP);
            GlobalPlayerHP.UpdateMaxHP(PlayerInfo.maxHP);
        }
    }

    void Update()
    {
        // 1. 키보드 입력 받기 (매 프레임)
        movement.x = Input.GetAxisRaw("Horizontal"); // -1 (A), 1 (D)
        movement.y = Input.GetAxisRaw("Vertical");   // -1 (S), 1 (W)

        // 2. 애니메이터 상태 업데이트
        UpdateAnimationState();
        
    }

    void FixedUpdate()
    {
        // 3. 물리 엔진으로 실제 이동 (고정 프레임마다)
        // .normalized: 대각선 이동 시 속도가 빨라지는 것을 방지
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    /// <summary>
    /// 입력 값에 따라 애니메이터의 파라미터를 조절합니다.
    /// </summary>
    void UpdateAnimationState()
    {
        // 1. 움직임이 있는지 확인 (벡터 크기가 0.1보다 큰가)
        if (movement.magnitude > 0.1f)
        {
            // "isWalking" 스위치를 켠다 (Walking 블렌드 트리로 전환)
            anim.SetBool("isRun", true);

            // 8방향 입력을 4방향 애니메이션으로 변환 (좌/우 우선)
            // (대각선 입력 시 좌/우 애니메이션을 재생)
            if (Mathf.Abs(movement.x) > 0.1f) // 좌/우 입력이 있다면
            {
                lastMoveDirection = new Vector2(movement.x, 0).normalized;
            }
            else // 상/하 입력만 있다면
            {
                lastMoveDirection = new Vector2(0, movement.y).normalized;
            }

            // 애니메이터에 현재 방향 값 전달
            anim.SetFloat("moveX", lastMoveDirection.x);
            anim.SetFloat("moveY", lastMoveDirection.y);
        }
        else // 2. 멈춘 경우
        {
            // "isWalking" 스위치를 끈다 (Idle 블렌드 트리로 전환)
            anim.SetBool("isRun", false);

            // 중요: 멈췄을 때는 moveX, moveY 값을 변경하지 않습니다!
            // → 그러면 Animator가 마지막으로 입력된 'lastMoveDirection' 값을
            //   기억하고, 알맞은 'stand' (대기) 애니메이션을 재생합니다.
        }
    }
}