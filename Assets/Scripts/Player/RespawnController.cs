using UnityEngine;

public class RespawnController : MonoBehaviour
{
    [SerializeField] private float fallDeathY = -5f;

    private Vector3 respawnPosition;
    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
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
        // 사망 기준보다 높은지 체크하고 아니면 대기
        if (transform.position.y > fallDeathY) return;

        Respawn();
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
    }
}