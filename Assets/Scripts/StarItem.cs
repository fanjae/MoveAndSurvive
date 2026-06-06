using UnityEngine;

public class StarItem : MonoBehaviour
{
    [SerializeField] private int scoreValue = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        ScoreManager.Instance.AddScore(scoreValue);

        Destroy(gameObject);
    }
}