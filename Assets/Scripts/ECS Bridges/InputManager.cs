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
        Instance = this;

        _input = new CharacterInput();
    }

    public void ConsumePause()
    {
        PausePressed = false;
    }

    private void Update()
    {
        MoveDirection = _input.Player.Move.ReadValue<Vector2>();

        PausePressed = _input.UI.Pause.WasPressedThisFrame();
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}