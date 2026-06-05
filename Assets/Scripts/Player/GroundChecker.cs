using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 checkSize = new Vector2(0.5f, 0.15f);

    // 현재 플레이어가 바닥에 닿아 있는지 여부
    public bool IsGrounded { get; private set; }

    private void FixedUpdate()
    {
        // 현재 위치를 중심으로 박스 영역을 검사해서
        // groundLayer에 해당하는 Collider2D가 있으면 true
        IsGrounded = Physics2D.OverlapBox(transform.position,checkSize,0f,groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, checkSize);
    }
}