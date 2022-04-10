using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private InputManager inputManager;
    public static CameraController singleton;
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

    #region MonoBehaviors
    private void Awake()
    {
        singleton = this;
        cameraAction = new CameraControlActions();
        cameraTransform = GetComponentInChildren<Camera>().transform;
    }

    private void Start()
    {
        inputManager = InputManager.singleton;
        zoomHeight = defaultZoomHeight;
        cameraTransform.LookAt(transform);
        lastPosition = transform.position;
    }
    #endregion

    #region Public Functions

    /// <summary>
    /// Updates all camera movement.
    /// </summary>
    /// <param name="delta">Time.DeltaTime</param>
    public void UpdateCameraController(float delta) {
        UpdateTargetPosition();
        CheckMouseAtScreenEdge();
        DragCamera();
        UpdateVelocity();
        UpdateCameraPosition(delta);
        UpdateBasePosition(delta);
        ZoomCamera();
    }
    public void RotateCamera(float delta)
    {
        transform.rotation = Quaternion.Euler(0f, delta * maxRotationSpeed + transform.rotation.eulerAngles.y, 0f);
    }

    #endregion

    #region Private Functions
    private void UpdateVelocity() {
        horizontalVelocity = (transform.position - lastPosition) / Time.deltaTime;
        horizontalVelocity.y = 0;
        lastPosition = transform.position;
    }

    private void UpdateTargetPosition() {
        Vector3 inputValue = inputManager.horizontal * GetCameraRight() + inputManager.vertical * GetCameraForward();
        inputValue = inputValue.normalized;
        if (inputValue.sqrMagnitude > 0.1f) {
            targetPosition += inputValue;
        }
    }

    private void UpdateBasePosition(float delta) {
        if (targetPosition.sqrMagnitude > 0.1f)
        {
            speed = Mathf.Lerp(speed, maxSpeed, delta * acceleration);
            transform.position += targetPosition * speed * delta;
        }
        else {
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, delta * damping);
            transform.position += horizontalVelocity * delta;
        }
        targetPosition = Vector3.zero;
    }


    private void ZoomCamera()
    {
        float value = -inputManager.mouseScroll / 100f;
        if (Mathf.Abs(value) > 0.1f)
        {
            zoomHeight = cameraTransform.localPosition.y + value * stepSize;
            if (zoomHeight < minHeight)
            {
                zoomHeight = minHeight;
            }
            else if (zoomHeight > maxHeight)
            {
                zoomHeight = maxHeight;
            }
        }
    }

    private void UpdateCameraPosition(float delta) {
        Vector3 zoomTarget = new Vector3(cameraTransform.localPosition.x, zoomHeight, cameraTransform.localPosition.z);
        zoomTarget -= zoomSpeed * (zoomHeight - cameraTransform.localPosition.y) * Vector3.forward;
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, zoomTarget, delta * zoomDamping);
        cameraTransform.LookAt(transform);
    }

    private void CheckMouseAtScreenEdge() {
        if (!useScreenEdge) return;
        Vector3 moveDirection = Vector3.zero;

        if (inputManager.mousePositionX < edgeTolerance * Screen.width)
        {
            moveDirection += -GetCameraRight();
        }
        else if (inputManager.mousePositionX > (1f - edgeTolerance) * Screen.width)
        {
            moveDirection += GetCameraRight();
        }
        else if (inputManager.mousePositionY < edgeTolerance * Screen.height) { 
            moveDirection += -GetCameraForward();
        }
        else if (inputManager.mousePositionY > (1f - edgeTolerance) * Screen.height)
        {
            moveDirection += GetCameraForward();
        }

        targetPosition += moveDirection;
    }

    private void DragCamera() {
        if (!Mouse.current.rightButton.isPressed) { return; }

        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Vector2 mousePosition = new Vector2(inputManager.mousePositionX, inputManager.mousePositionY);
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
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

    private Vector3 GetCameraRight()
    {
        Vector3 right = cameraTransform.right;
        right.y = 0f;
        return right;
    }

    private Vector3 GetCameraForward()
    {
        Vector3 forward = cameraTransform.forward;
        forward.y = 0f;
        return forward;
    }
    #endregion
}
