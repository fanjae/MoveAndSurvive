using UnityEngine;
using UnityEngine.InputSystem;

public class ClearGoal : MonoBehaviour
{
    [SerializeField] private GameObject clearPanel;

    private InputAction exitAction;
    private bool isCleared = false;

    private void Awake()
    {
        exitAction = InputSystem.actions.FindAction("Exit");

        if (exitAction == null) return;
    }

    private void Start()
    {
        clearPanel.SetActive(false);
    }

    private void Update()
    {
        if (!isCleared) return;
        if (exitAction == null) return;

        if (exitAction.WasPressedThisFrame())
        {
            QuitGame();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCleared) return;
        if (!other.CompareTag("Player")) return;

        isCleared = true;
        clearPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    private void QuitGame()
    {
        Time.timeScale = 1f;

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}