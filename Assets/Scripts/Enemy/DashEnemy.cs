using UnityEngine;

public class DashEnemy : MonoBehaviour
{
    private enum State
    {
        Idle,
        Dashing,
        Cooldown
    }

    [Header("Visual")]
    [SerializeField] private GameObject eyeObject;

    [Header("Detect")]
    [SerializeField] private float sightRange = 5f;
    [SerializeField] private LayerMask playerLayer;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 6f;
    [SerializeField] private float dashDuration = 3f;
    [SerializeField] private float cooldown = 1f;

    [Header("Ground Check")]
    [SerializeField] private Transform leftEdgeCheck;
    [SerializeField] private Transform rightEdgeCheck;
    [SerializeField] private float edgeCheckRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private State state = State.Idle;

    private Vector2 dashDirection;
    private float timer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() 
    {
        // 현재 상태에 따라 다르게 동작
        switch (state)
        {
            case State.Idle:
                DetectPlayer();
                break;

            case State.Dashing:
                Dash();
                break;

            case State.Cooldown:
                Cooldown();
                break;
        }

        // 돌진 중일때만 눈 활성화
        if (eyeObject != null)
            eyeObject.SetActive(state == State.Dashing);
    }

    private void DetectPlayer()
    {
        // 현재 위치 기준으로 원형 범위 내 플레이어를 탐지 하도록 처리
        Collider2D hit = Physics2D.OverlapCircle(transform.position,sightRange,playerLayer);

        if (hit == null) return;

        // 플레이어 방향 계산
        Vector2 direction = hit.transform.position - transform.position;
        dashDirection = direction.normalized;

        // 돌진 상태 전환
        timer = dashDuration;
        state = State.Dashing;
    }

    private void Dash()
    {
        // 진행 방향 앞 바닥 없으면 멈춤.
        if (!IsGroundAhead())
        {
            rb.linearVelocity = Vector2.zero;
            timer = cooldown;
            state = State.Cooldown;
            return;
        }

        // x축 방향으로 돌진, y축은 원래 기존 대로
        rb.linearVelocity = new Vector2(dashDirection.x * dashSpeed,rb.linearVelocity.y);

        timer -= Time.fixedDeltaTime;

        // 돌진 끝나면 정지 후, 쿨다운으로 전환
        if (timer <= 0f)
        {
            rb.linearVelocity = Vector2.zero;
            timer = cooldown;
            state = State.Cooldown;
        }
    }

    private bool IsGroundAhead()
    {
        // 돌진 방향 따라서 방향 체크
        Transform check = dashDirection.x > 0 ? rightEdgeCheck : leftEdgeCheck;

        // 선택한 위치에 바닥 레이어 검사
        return Physics2D.OverlapCircle(check.position,edgeCheckRadius,groundLayer);
    }

    private void Cooldown()
    {
        // 쿨다운 시간 감소
        timer -= Time.fixedDeltaTime;

        // 탐지 상태로 전환
        if (timer <= 0f)
        {
            state = State.Idle;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}