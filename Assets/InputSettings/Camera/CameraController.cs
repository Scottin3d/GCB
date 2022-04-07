using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private CameraControlActions cameraAction;
    private InputAction movement;
    private Transform cameraTransform;

    // horizontal motion
    [SerializeField] 
    float maxSpeed = 5f;
    float speed;
    [SerializeField]
    float acceleration = 10f;
    [SerializeField]
    float damping = 15f;

    // vertical motion - zooming
    [SerializeField]
    float stepSize = 2f;
    [SerializeField]
    float zoomDamping = 7.5f;
    [SerializeField]
    float minHeight = 5f;
    [SerializeField]
    float maxHeight = 50f;
    [SerializeField]
    float zoomSpeed = 2f;

    // rotation
    [SerializeField]
    float maxRotationSpeed = 1f;

    // screen edge motion
    [SerializeField]
    [Range(0f, 0.1f)]
    float edgeTolerance = 0.05f;
    [SerializeField]
    bool useScreenEdge = true;

    Vector3 targetPosition;
    float zoomHeight;
    float defaultZoomHeight = 10f;
    Vector3 horizontalVelocity;
    Vector3 lastPosition;
    Vector3 startDrag;


    private void Awake()
    {
        cameraAction = new CameraControlActions();
        cameraTransform = GetComponentInChildren<Camera>().transform;
    }

    private void Update()
    {
        GetKeyboardMovement();
        if (useScreenEdge)
        {
            CheckMouseAtScreenEdge();
        }
        DragCamera();

        UpdateVelocity();
        UpdateCameraPosition();
        UpdateBasePosition();
    }

    private void OnEnable()
    {
        zoomHeight = defaultZoomHeight;
        cameraTransform.LookAt(transform);

        lastPosition = transform.position;
        movement = cameraAction.CameraActionMap.Movement;
        cameraAction.CameraActionMap.RotateCamera.performed += RotateCamera;
        cameraAction.CameraActionMap.ZoomCamera.performed += ZoomCamera;
        cameraAction.CameraActionMap.Enable();
    }

    

    private void OnDisable()
    {
        cameraAction.CameraActionMap.RotateCamera.performed -= RotateCamera;
        cameraAction.CameraActionMap.ZoomCamera.performed -= ZoomCamera;
        cameraAction.CameraActionMap.Disable();
    }

    private void UpdateVelocity() {
        horizontalVelocity = (transform.position - lastPosition) / Time.deltaTime;
        horizontalVelocity.y = 0;
        lastPosition = transform.position;
    }

    private void GetKeyboardMovement() {
        Vector3 inputValue = movement.ReadValue<Vector2>().x * GetCameraRight() + movement.ReadValue<Vector2>().y * GetCameraForward();
        inputValue = inputValue.normalized;
        if (inputValue.sqrMagnitude > 0.1f) {
            targetPosition += inputValue;
        }
    }

    private Vector3 GetCameraRight() {
        Vector3 right = cameraTransform.right;
        right.y = 0f;
        return right;
    }

    private Vector3 GetCameraForward() {
        Vector3 forward = cameraTransform.forward;
        forward.y = 0f;
        return forward;
    }

    private void UpdateBasePosition() {
        if (targetPosition.sqrMagnitude > 0.1f)
        {
            speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime * acceleration);
            transform.position += targetPosition * speed * Time.deltaTime;
        }
        else {
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, Time.deltaTime * damping);
            transform.position += horizontalVelocity * Time.deltaTime;
        }
        targetPosition = Vector3.zero;
    }

    private void RotateCamera(InputAction.CallbackContext inputValue)
    {
        if (!Mouse.current.middleButton.isPressed) { return; }
        float value = inputValue.ReadValue<Vector2>().x;
        transform.rotation = Quaternion.Euler(0f, value * maxRotationSpeed + transform.rotation.eulerAngles.y, 0f);
    }

    private void RotateCamera(float inputValue)
    {
        if (!Mouse.current.middleButton.isPressed) { return; }
        transform.rotation = Quaternion.Euler(0f, inputValue * maxRotationSpeed + transform.rotation.eulerAngles.y, 0f);
    }

    private void ZoomCamera(InputAction.CallbackContext inputValue)
    {
        float value = -inputValue.ReadValue<Vector2>().y / 100f;
        if (Mathf.Abs(value) > 0.1f) {
            zoomHeight = cameraTransform.localPosition.y + value * stepSize;
            if (zoomHeight < minHeight) {
                zoomHeight = minHeight;
            } else if (zoomHeight > maxHeight) {
                zoomHeight = maxHeight;
            }
        }
    }

    private void UpdateCameraPosition() {
        Vector3 zoomTarget = new Vector3(cameraTransform.localPosition.x, zoomHeight, cameraTransform.localPosition.z);
        zoomTarget -= zoomSpeed * (zoomHeight - cameraTransform.localPosition.y) * Vector3.forward;
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, zoomTarget, Time.deltaTime * zoomDamping);
        cameraTransform.LookAt(transform);
    }

    private void CheckMouseAtScreenEdge() {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 moveDirection = Vector3.zero;

        if (mousePosition.x < edgeTolerance * Screen.width)
        {
            moveDirection += -GetCameraRight();
        }
        else if (mousePosition.x > (1f - edgeTolerance) * Screen.width)
        {
            moveDirection += GetCameraRight();
        }
        else if (mousePosition.y < edgeTolerance * Screen.height) { 
            moveDirection += -GetCameraForward();
        }
        else if (mousePosition.y > (1f - edgeTolerance) * Screen.height)
        {
            moveDirection += GetCameraForward();
        }

        targetPosition += moveDirection;
    }

    private void DragCamera() {
        if (!Mouse.current.rightButton.isPressed) { return; }

        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (plane.Raycast(ray, out float distance)) {
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                startDrag = ray.GetPoint(distance);
            }
            else {
                targetPosition += startDrag - ray.GetPoint(distance);
            }
        }
    }
}
