using UnityEngine;

public class JumpChaseEnemy : MonoBehaviour
{
    // 적의 현재 상태
    private enum State
    {
        Idle,
        Chasing,
        Cooldown
    }

    [Header("Visual")]
    [SerializeField] private GameObject eyeObject;

    [Header("Detect")]
    [SerializeField] private float sightRange = 5f;
    [SerializeField] private LayerMask playerLayer;

    [Header("Chase")]
    [SerializeField] private float chaseSpeed = 3f;
    [SerializeField] private float stopDistance = 0.3f;

    [Header("Jump")]
    [SerializeField] private float jumpPower = 16f;
    [SerializeField] private float jumpRangeX = 2.5f;
    [SerializeField] private float playerAboveHeight = 1.2f;
    [SerializeField] private float playerJumpVelocityThreshold = 0.1f;
    [SerializeField] private float jumpCooldown = 1f;

    [Header("Jump Space Check")]
    [SerializeField] private float jumpSpaceHeight = 1.2f;
    [SerializeField] private float jumpSpaceWidth = 0.6f;
    [SerializeField] private float jumpSpaceYOffset = 0.6f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform leftEdgeCheck;
    [SerializeField] private Transform rightEdgeCheck;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private float edgeCheckRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private State state = State.Idle;

    private Transform player;
    private Rigidbody2D playerRb;

    private float timer;
    private float jumpTimer;
    private bool isJumping;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // 점프 쿨다운 감소
        if (jumpTimer > 0f) jumpTimer -= Time.fixedDeltaTime; 

        // 바닥에 닿고 하강 또는 정지 상태일 때 점프 해제
        if (IsGrounded() && rb.linearVelocity.y <= 0f) isJumping = false;

        switch (state)
        {
            case State.Idle:
                DetectPlayer();
                break;

            case State.Chasing:
                // 플레이어의 상태에 따라 같이 점프
                TryJump();

                // 적이 공중에 있을때는 좌우 추적 이동 안함
                if (!isJumping)
                    Chase();

                break;

            case State.Cooldown:
                Cooldown();
                break;
        }

        // 추적 모드일때 눈 활성화
        if (eyeObject != null)
            eyeObject.SetActive(state == State.Chasing);
    }

    private void DetectPlayer()
    {
        Collider2D hit = Physics2D.OverlapCircle(
            transform.position,
            sightRange,
            playerLayer
        );

        if (hit == null)
            return;

        player = hit.transform;
        playerRb = hit.GetComponent<Rigidbody2D>();

        state = State.Chasing;
    }

    private void Chase()
    {
        if (player == null) // 플레이어가 사라진 경우 정지 후 대기 상태
        {
            StopHorizontalMove();
            state = State.Idle;
            return;
        }

        float directionX = player.position.x - transform.position.x;
        float distanceX = Mathf.Abs(directionX);

        if (distanceX <= stopDistance) // 거리가 가까우면 이동X
        {
            StopHorizontalMove();
            return;
        }

        float moveDirectionX = Mathf.Sign(directionX); 

        if (!IsGroundAhead(moveDirectionX)) // 바닥이 없으면 낙하 방지 위해 정지
        {
            StopHorizontalMove();
            timer = 0.5f;
            state = State.Cooldown;
            return;
        }

        rb.linearVelocity = new Vector2(
            moveDirectionX * chaseSpeed,
            rb.linearVelocity.y
        );
    }

    private void TryJump()
    {
        if (isJumping) return;

        if (jumpTimer > 0f) return;

        if (!IsGrounded()) return;

        if (!HasJumpSpace()) return;

        if (!IsPlayerInJumpRange()) return;

        if (!IsPlayerJumping()) return;

        isJumping = true;

        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            jumpPower
        );

        jumpTimer = jumpCooldown;
    }

    private bool IsPlayerInJumpRange() 
    {
        float distanceX = Mathf.Abs(player.position.x - transform.position.x);
        float distanceY = player.position.y - transform.position.y;

        // x축 범위 밖이면 점프하지 않음
        if (distanceX > jumpRangeX) return false;

        // 플레이어가 충분히 위에 있어야만 점프.
        if (distanceY < playerAboveHeight) return false;

        return true;
    }

    private bool IsPlayerJumping()
    {
        // 플레이어의 Y축 속도가 일정 이상보다 크면 점프 중인 것으로 판단
        return playerRb.linearVelocity.y > playerJumpVelocityThreshold;
    }

    private bool IsGrounded()
    {
        if (groundCheck == null) return false;

        // 바닥 레이어 검사
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private bool IsGroundAhead(float moveDirectionX)
    {
        // 이동 방향 따라서 왼쪽/오른쪽 위치 체크
        Transform check = moveDirectionX > 0 ? rightEdgeCheck : leftEdgeCheck;

        if (check == null) return false;

        return Physics2D.OverlapCircle(
            check.position,
            edgeCheckRadius,
            groundLayer
        );
    }

    private bool HasJumpSpace()
    {
        // 적 머리 위 검사 위치
        Vector2 checkPosition = new Vector2(
            transform.position.x,
            transform.position.y + jumpSpaceYOffset
        );

        Vector2 checkSize = new Vector2(
            jumpSpaceWidth,
            jumpSpaceHeight
        );

        // 머리 위 영역에 바닥 레이어 체크해서 점프 가능 여부 확인
        return !Physics2D.OverlapBox(
            checkPosition,
            checkSize,
            0f,
            groundLayer
        );
    }

    private void StopHorizontalMove()
    {
        // Y축 속도는 유지하고 X축 이동만 정지
        rb.linearVelocity = new Vector2(0f,rb.linearVelocity.y);
    }

    private void Cooldown()
    {
        // 쿨다운 시간 감소
        timer -= Time.fixedDeltaTime;

        if (timer <= 0f)
        {
            state = State.Idle;
        }
    }
    private void OnDrawGizmosSelected()
    {
        // 플레이어 탐지 범위
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        // 바닥 및 낭떠러지
        Gizmos.color = Color.red;

        if (groundCheck != null)
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        if (leftEdgeCheck != null)
            Gizmos.DrawWireSphere(leftEdgeCheck.position, edgeCheckRadius);

        if (rightEdgeCheck != null)
            Gizmos.DrawWireSphere(rightEdgeCheck.position, edgeCheckRadius);
    }
}