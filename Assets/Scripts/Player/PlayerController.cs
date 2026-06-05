using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 플레이어 애니메이션 상태
    private enum PlayerState
    {
        Idle = 0,
        Move = 1,
        Jump = 2
    }

    [Header("Move")]
    [SerializeField] private float moveSpeed = 5.0f;

    [Header("Jump")]
    [SerializeField] private float jumpPower = 12.0f;

    [Header("Ground")]
    [SerializeField] private GroundChecker groundChecker;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private PlayerState currentState = PlayerState.Idle;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Jump();
        Flip();
        UpdateState();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move() // 이동 처리
    {
        float moveX = InputManager.Movement.x;

        // X축 속도는 입력값 따라 변경, Y축은 그대로
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);
    }

    private void Flip()
    {
        float moveX = InputManager.Movement.x;

        // FlipX를 이용해서 스프라이트 회전 처리
        if (moveX > 0) spriteRenderer.flipX = false;
        else if (moveX < 0) spriteRenderer.flipX = true;
    }

    private void UpdateState()
    {
        // 다음 상태 계산
        PlayerState nextState = GetState();

        // 상태 바뀌지 않은 경우 애니메이션 갱신 생략
        if (currentState == nextState) return;

        currentState = nextState;

        if (animator != null)
            animator.SetInteger("State", (int)currentState);
    }

    private void Jump()
    {
        if (!InputManager.IsJump) return;

        if (!groundChecker.IsGrounded) return;

        // X축 속도는 유지하고, Y축 속도만 점프 힘으로 변경
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
    }

    private PlayerState GetState()
    {
        // 바닥에 닿아 있지 않으면 점프 상태 
        if (!groundChecker.IsGrounded) return PlayerState.Jump;

        // 좌우 입력이 있으면 이동 상태
        if (Mathf.Abs(InputManager.Movement.x) > 0.01f) return PlayerState.Move;

        // 이외 대기 상태
        return PlayerState.Idle;
    }

    public void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;
    }
}