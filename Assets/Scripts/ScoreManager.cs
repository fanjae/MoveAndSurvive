using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI scoreText;

    private int score = 0;
    public int Score => score;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        UpdateUI();
    }

    public void AddScore(int value) // 점수 증가
    {
        score += value;
        UpdateUI();
    }

    private void UpdateUI() // 점수 업데이트
    {
        scoreText.text = $"x {score}";
    }

    
}