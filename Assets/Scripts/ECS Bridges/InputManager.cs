using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private CharacterInput _input;

    public Vector2 MoveDirection { get; private set; }

    public bool PausePressed { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _input = new CharacterInput();
        _input.Enable();
    }

    private void Update()
    {
        MoveDirection = _input.Player.Move.ReadValue<Vector2>();

        PausePressed = _input.UI.Pause.WasPressedThisFrame();
    }

    public void ConsumePause()
    {
        PausePressed = false;
    }
}