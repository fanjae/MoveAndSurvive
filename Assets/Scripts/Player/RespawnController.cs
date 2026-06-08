using System.Collections;
using UnityEngine;

public class RespawnController : MonoBehaviour
{
    [SerializeField] private float fallDeathY = -5f;
    [SerializeField] private Animator animator;

    [Header("Death Motion")]
    [SerializeField] private float deathFreezeDelay = 0.3f;
    [SerializeField] private float deathJumpPower = 8f;
    [SerializeField] private float deathDelay = 0.5f;

    private Vector3 respawnPosition;
    private PlayerController playerController;
    private Rigidbody2D rb;

    private float gravityScale;

    private bool isRespawning = false;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();

        gravityScale = rb.gravityScale;

        if (animator == null)
            animator = GetComponent<Animator>();
    }
    private void Start()
    {
        // 게임 시작 시 현재 위치를 초기 리스폰 위치로
        respawnPosition = transform.position;

    }

    private void Update()
    {
        // 낙사 체크
        CheckFallDeath();
    }

    private void CheckFallDeath()
    {
        if (transform.position.y > fallDeathY) return;

        StartCoroutine(RespawnRoutine());
    }

    public void SetRespawn(float x)
    {
        // 체크포인트의 X좌표만 리스폰 위치에 반영
        respawnPosition = new Vector3(x,respawnPosition.y,respawnPosition.z);
    }

    private void Respawn()
    {
        // 플레이어 속도 초기화 및 저장된 리스폰 위치로 플레이어 이동.
        playerController.StopMovement();
        transform.position = respawnPosition;

        if (animator != null)
            animator.SetTrigger("Respawn");
    }

    private IEnumerator RespawnRoutine()
    {
        isRespawning = true;

        // 죽은 직후 그 자리에서 잠깐 정지
        playerController.StopMovement();
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;

        if (animator != null)
            animator.SetTrigger("Death");

        yield return new WaitForSeconds(deathFreezeDelay);

        // 정지 끝난 뒤 사망 연출
        rb.gravityScale = gravityScale;
        rb.linearVelocity = new Vector2(0f, deathJumpPower);

        yield return new WaitForSeconds(deathDelay);

        // 리스폰 위치로 이동
        playerController.StopMovement();
        transform.position = respawnPosition;

        // 리스폰 애니메이션
        if (animator != null)
            animator.SetTrigger("Respawn");

        isRespawning = false;
    }
}