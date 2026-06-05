using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private enum MoveMode
    {
        LeftRight,
        UpDown
    }

    [SerializeField] private MoveMode moveMode = MoveMode.LeftRight;
    [SerializeField] private float moveDistance = 3f;
    [SerializeField] private float moveSpeed = 1f;

    private Vector3 startPosition;
    private Vector3 targetPosition;

    private void Start()
    {
        startPosition = transform.position;

        Vector3 direction = GetDirection();
        targetPosition = startPosition + direction * moveDistance;
    }

    private void Update()
    {
        float t = Mathf.PingPong(Time.time * moveSpeed, 1f);
        transform.position = Vector3.Lerp(startPosition, targetPosition, t);
    }

    private Vector3 GetDirection()
    {
        switch (moveMode)
        {
            case MoveMode.LeftRight:
                return Vector3.right;

            case MoveMode.UpDown:
                return Vector3.up;

            default:
                return Vector3.right;
        }
    }
}