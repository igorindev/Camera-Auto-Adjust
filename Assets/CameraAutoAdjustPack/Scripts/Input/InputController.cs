using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] TPSCameraController.CameraController cameraController;
    [SerializeField] Player.Movement.PlayerController movement;

    PlayerInput inputActions;

    void Awake()
    {
        inputActions = new PlayerInput();

        inputActions.Player.Camera.performed += ctx => cameraController.Look(ctx.ReadValue<Vector2>());
        inputActions.Player.Camera.canceled += ctx => cameraController.Look(Vector2.zero);

        inputActions.Player.Movement.performed += ctx => movement.Movement(ctx.ReadValue<Vector2>());
        inputActions.Player.Movement.canceled += ctx => movement.Movement(Vector2.zero);

        inputActions.Player.Jump.performed += ctx => movement.Jump(true);
        inputActions.Player.Jump.canceled += ctx => movement.Jump(false);

        inputActions.Player.Dash.performed += ctx => movement.Dash();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}
