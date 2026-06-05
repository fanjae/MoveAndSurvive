using UnityEngine;

public class IceBreakable : MonoBehaviour
{
    [SerializeField] private float breakDelay = 0f;

    private bool isBroken = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 중복 실행 방지
        if (isBroken) return;

        if (!collision.gameObject.CompareTag("Player")) return;

        // 플레이어가 위에서 밟은 경우만 처리
        // ContactPoint2D로 충돌 지점 및 방향 파악
        foreach (ContactPoint2D contact in collision.contacts)
        {
            // 위에서 밟은 상황
            if (contact.normal.y < -0.5f)
            {
                isBroken = true;
                Destroy(gameObject, breakDelay);
                break;
            }
        }
    }
}