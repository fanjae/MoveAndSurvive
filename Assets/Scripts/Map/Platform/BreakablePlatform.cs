using UnityEngine;

public class IceBreakable : MonoBehaviour
{
    [SerializeField] private float breakDelay = 0f;

    private bool isBroken = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isBroken)
            return;

        if (!collision.gameObject.CompareTag("Player"))
            return;

        // 플레이어가 위에서 밟은 경우만 처리
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y < -0.5f)
            {
                isBroken = true;
                Destroy(gameObject, breakDelay);
                break;
            }
        }
    }
}