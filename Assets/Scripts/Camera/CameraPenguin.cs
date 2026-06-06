using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollowX : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private SpriteRenderer targetSprite;

    [SerializeField] private float lookAheadDistance = 2f;
    [SerializeField] private float smoothTime = 0.25f;

    private float velocityX;

    private void LateUpdate()
    {
        Vector3 pos = transform.position;

        // 펭귄의 위치에 따라서, 카메라의 보는 앞쪽을 변형할 목적
        float direction = targetSprite.flipX ? -1f : 1f;
        float targetX = target.position.x + direction * lookAheadDistance;

        // 보이는 거리
        pos.x = Mathf.SmoothDamp(pos.x,targetX,ref velocityX,smoothTime);

        transform.position = pos;
    }
}