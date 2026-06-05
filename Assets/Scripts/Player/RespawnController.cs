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
        respawnPosition = transform.position;
    }

    private void Update()
    {
        CheckFallDeath();
    }

    private void CheckFallDeath()
    {
        if (transform.position.y > fallDeathY) return;

        Respawn();
    }

    public void SetRespawn(float x)
    {
        respawnPosition = new Vector3(x,respawnPosition.y,respawnPosition.z);
    }

    private void Respawn()
    {
        playerController.StopMovement();
        transform.position = respawnPosition;
    }
}