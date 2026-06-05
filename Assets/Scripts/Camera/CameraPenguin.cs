using UnityEngine;

public class CameraFollowX : MonoBehaviour
{
    [SerializeField] private Transform target;
    private float xOffset;

    private void Awake()
    {
        xOffset = transform.position.x - target.position.x;
    }

    private void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.x = target.position.x + xOffset;
        transform.position = pos;
    }
}