using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private bool isActivated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isActivated)
            return;

        // Player 태그일 경우에만 발동        
        if (!other.CompareTag("Player"))
            return;

        RespawnController respawnController = other.GetComponent<RespawnController>();
        if (respawnController == null) return;

        // 체크포인트의 x좌표만 변경.
        respawnController.SetRespawn(transform.position.x);

        // 체크포인트 활성화
        isActivated = true;

        Destroy(gameObject);
    }
}