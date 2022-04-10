using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputHandler : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;
    private float mouseScrollY;
    private float mouseScrollX;

    PlayerControls inputActions;
    CameraHandler cameraHandler;
    CameraController cameraController;

    Vector2 movementInput;
    Vector2 cameraInput;
    float cameraScroll;
    private void Start()
    {
        cameraHandler = CameraHandler.singleton;
    }

    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;
        if (cameraHandler != null) {
            cameraHandler.FollowTarget(delta);
            cameraHandler.HandleCameraRotationKeyPress(delta);
            //cameraHandler.HandleCameraRotation(delta, mouseX, mouseY);
            cameraHandler.ZoomCamera(delta, mouseScrollY);
            cameraHandler.UpdateCameraPosition();
            cameraScroll = 0f;
        }
    }

    public void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerControls();
            inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            inputActions.PlayerMovement.ZoomCamera.performed += z => cameraScroll = z.ReadValue<float>();

        }
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void TickInput(float delta)
    {
        
        MoveInput(delta);
    }

    private void MoveInput(float delta)
    {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
        mouseScrollY = cameraScroll;
    }

}

