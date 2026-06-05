using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private bool isActivated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isActivated)
            return;

        if (!other.CompareTag("Player"))
            return;

        RespawnController respawnController =
            other.GetComponent<RespawnController>();

        if (respawnController == null)
            return;

        respawnController.SetRespawn(transform.position.x);

        isActivated = true;
        Destroy(gameObject);
    }
}