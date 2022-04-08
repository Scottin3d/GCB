using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager singleton;
    private PlayerControls inputActions;
    private CameraControlActions cameraAction;
   

    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;
    private float mouseScroll;

    Vector2 movementInput;
    Vector2 cameraRotateInput;
    Vector2 cameraScrollInput;

    // assign singleton
    private void Awake()
    {
        singleton = this;
    }


    private void OnEnable()
    {
        if (inputActions == null) { 
            inputActions = new PlayerControls();
            inputActions.CameraActionMap.Movement.performed += m => movementInput = m.ReadValue<Vector2>();
            inputActions.CameraActionMap.CameraRotateMouse.performed += c => cameraRotateInput = c.ReadValue<Vector2>();
            inputActions.CameraActionMap.CameraZoom.performed += z => cameraScrollInput = z.ReadValue<Vector2>();
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
        mouseX = cameraRotateInput.x;
        mouseY = cameraRotateInput.y;
        mouseScroll = cameraScrollInput.y;
    }

}
