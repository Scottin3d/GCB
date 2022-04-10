using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager singleton;
    private PlayerControls inputActions;
    private CameraControlActions cameraAction;
    private CameraController cameraController;

    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseDeltaX;
    public float mouseDeltaY;
    public float mouseScroll;
    public float mousePositionX;
    public float mousePositionY;
    public float keyRotate;
    Vector2 movementInput;
    Vector2 cameraRotateInput;
    float cameraScrollInput;
    Vector2 mousePositionInput;
    private bool qKeyInput;
    private bool eKeyInput;
    private bool middleMouseButtonDown;

    #region MonoBehaviors
    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        cameraController = CameraController.singleton;
    }

    private void OnEnable()
    {
        if (inputActions == null) { 
            inputActions = new PlayerControls();
            inputActions.CameraActionMap.Movement.performed += m => movementInput = m.ReadValue<Vector2>();
            inputActions.CameraActionMap.CameraRotateMouse.performed += c => cameraRotateInput = c.ReadValue<Vector2>();
            inputActions.CameraActionMap.CameraZoom.performed += z => cameraScrollInput = z.ReadValue<float>();
            inputActions.CameraActionMap.MousePosition.performed += mp => mousePositionInput = mp.ReadValue<Vector2>();
        }
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }
    #endregion

    #region Public Functions
    /// <summary>
    /// Called once per <c>FixedUpdate</c>.
    /// </summary>
    /// <param name="delta">Time.DeltaTime</param>
    public void TickInput(float delta)
    {
        MoveInput(delta);
        HandleCameraRotateInput();
    }

    /// <summary>
    /// Resets the zoom scroll value to 0 to prevent continuous scroll.
    /// </summary>
    public void ResetFlags() {
        qKeyInput = false;
        eKeyInput = false;
        middleMouseButtonDown = false;
        cameraScrollInput = 0f;
    }
    #endregion

    #region Private Functions
    private void MoveInput(float delta)
    {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseDeltaX = cameraRotateInput.x;
        mouseDeltaY = cameraRotateInput.y;
        mouseScroll = cameraScrollInput;
        mousePositionX = mousePositionInput.x;
        mousePositionY = mousePositionInput.y;
    }

    private void HandleCameraRotateInput()
    {
        qKeyInput = inputActions.CameraActionMap.CameraRotateLeft.IsPressed();
        eKeyInput = inputActions.CameraActionMap.CameraRotateRight.IsPressed();
        middleMouseButtonDown = inputActions.CameraActionMap.CameraMiddleMouse.IsPressed();

        if (qKeyInput)
        {
            cameraController.RotateCamera(1f);
        }
        if (eKeyInput)
        {
            cameraController.RotateCamera(-1f);
        }
        if (middleMouseButtonDown)
        {
            cameraController.RotateCamera(mouseDeltaX);
        }

    }
    #endregion
}
