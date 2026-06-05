using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // 발판 이동 방향 모드
    private enum MoveMode
    {
        LeftRight,
        UpDown
    }

    [SerializeField] private MoveMode moveMode = MoveMode.LeftRight;
    [SerializeField] private float moveDistance = 3f;
    [SerializeField] private float moveSpeed = 1f;

    private Rigidbody2D rb;
    private Vector2 startPosition;
    private Vector2 targetPosition;
    private Vector2 previousPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
    }

    private void Start()
    {
        startPosition = rb.position;
        targetPosition = startPosition + GetDirection() * moveDistance;
        previousPosition = rb.position;
    }

    private void FixedUpdate()
    {
        // 시작 위치와 목표 위치 사이 왕복할 위치 계산
        float t = Mathf.PingPong(Time.time * moveSpeed, 1f);
        Vector2 nextPosition = Vector2.Lerp(startPosition, targetPosition, t);


        // 발판이 이동한 거리
        Vector2 deltaPosition = nextPosition - previousPosition;

        // 발판 위 플레이어도 이동 처리
        MovePlayerOnPlatform(deltaPosition);

        // 발판 이동 및 다음 프레임 계산을 위해 위치 저장 
        rb.MovePosition(nextPosition);
        previousPosition = nextPosition;
    }

    private void MovePlayerOnPlatform(Vector2 deltaPosition)
    {
        // 발판 위쪽에 있는 Collider2D Check
        Collider2D[] hits = Physics2D.OverlapBoxAll(
            transform.position + Vector3.up * 0.6f,
            new Vector2(transform.localScale.x, 0.2f),
            0f
        );

        foreach (Collider2D hit in hits)
        {
            // Player 태그만 처리
            if (!hit.CompareTag("Player")) continue;

            Rigidbody2D playerRb = hit.GetComponent<Rigidbody2D>();

            // Rigidbody2D가 없으면 이동 불가하므로 무시한다.
            if (playerRb == null) continue;

            // 플레이어를 발판 이동량 만큼 이동시킨다.
            playerRb.position += deltaPosition;
        }
    }

    private Vector2 GetDirection()
    {
        // 선택한 이동 모드에 따라서 이동 방향을 체크
        switch (moveMode)
        {
            case MoveMode.LeftRight:
                return Vector2.right;

            case MoveMode.UpDown:
                return Vector2.up;

            default:
                return Vector2.right;
        }
    }

    private void OnDrawGizmos() // 에디터 테스트 용
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireCube(
            transform.position + Vector3.up * 0.6f,
            new Vector2(transform.localScale.x, 0.2f)
        );
    }
}