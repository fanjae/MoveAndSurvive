using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static Vector2 Movement { get; private set; } = Vector2.zero;
    public static bool IsJump { get; private set; } = false;

    private InputAction moveAction;
    private InputAction jumpAction;

    private void Awake()
    {
        // Input System 액션 찾기
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    void Update()
    {
        // 입력값 읽기
        Movement = moveAction.ReadValue<Vector2>();

        IsJump = jumpAction.WasPressedThisFrame();
    }
}
