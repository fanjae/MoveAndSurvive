using UnityEngine;

public class PlayerController : MonoBehaviour
{
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

    private void Move()
    {
        float moveX = InputManager.Movement.x;

        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);
    }

    private void Flip()
    {
        float moveX = InputManager.Movement.x;

        if (moveX > 0) spriteRenderer.flipX = false;
        else if (moveX < 0) spriteRenderer.flipX = true;
    }

    private void UpdateState()
    {
        PlayerState nextState = GetState();

        if (currentState == nextState)
            return;

        currentState = nextState;

        if (animator != null)
            animator.SetInteger("State", (int)currentState);
    }

    private void Jump()
    {
        if (!InputManager.IsJump)
            return;

        Debug.Log("Jump Input");

        if (!groundChecker.IsGrounded)
        {
            Debug.Log("Jump Failed: Not Grounded");
            return;
        }
        Debug.Log("Jump Success");

        rb.linearVelocity = new Vector2(rb.linearVelocity.x,jumpPower);
    }

    private PlayerState GetState()
    {
        if (!groundChecker.IsGrounded)
            return PlayerState.Jump;

        if (Mathf.Abs(InputManager.Movement.x) > 0.01f)
            return PlayerState.Move;

        return PlayerState.Idle;
    }

    public void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;
    }
}
