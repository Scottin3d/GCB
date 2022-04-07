using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraHandler : MonoBehaviour
{
    public Transform targetTransform;
    public Transform cameraTransform;
    public Transform cameraPivotTransform;
    private Transform myTransform;
    private Vector3 cameraTransformPosition;
    private LayerMask ignorLayers;

    public static CameraHandler singleton;

    public float lookSpeed = 0.1f;
    public float followSpeed = 0.1f;
    public float pivotSpeed = 0.03f;

    private float defaultPosition;
    private float lookAngle;
    private float pivotAngle;
    public float minPivot = -35f;
    public float maxPivot = 35f;

    // rotation
    [SerializeField]
    float maxRotationSpeed = 1f;

    // vertical motion - zooming
    [SerializeField]
    float stepSize = 2f;
    [SerializeField]
    float zoomDamping = 7.5f;
    [SerializeField]
    float minHeight = 1f;
    [SerializeField]
    float maxHeight = 20f;
    [SerializeField]
    float zoomSpeed = 2f;
    [SerializeField]
    float zoomHeight;
    float defaultZoomHeight = 5f;

    private void Awake()
    {
        singleton = this;
        myTransform = transform;
        defaultPosition = cameraTransform.localPosition.z;
        ignorLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
    }

    public void FollowTarget(float delta) {
        Vector3 zoomTarget = new Vector3(targetTransform.localPosition.x, zoomHeight, targetTransform.localPosition.z);
        zoomTarget -= zoomSpeed * (zoomHeight - myTransform.localPosition.y) * Vector3.forward;

        Vector3 targetPosition = Vector3.Lerp(myTransform.position, zoomTarget, delta / followSpeed);
        myTransform.position = targetPosition;
    }

    public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput) {
        lookAngle += (mouseXInput * lookSpeed) / delta;
        pivotAngle -= (mouseYInput * pivotSpeed) / delta;
        pivotAngle = Mathf.Clamp(pivotAngle, minPivot, maxPivot);

        Vector3 rotation = Vector3.zero;
        rotation.y = lookAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        myTransform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;

        targetRotation = Quaternion.Euler(rotation);
        cameraPivotTransform.localRotation = targetRotation;
    }

    public void HandleCameraRotationKeyPress(float delta) {
        float mouseXInput = 0f;
        float mouseYInput = 0f;
        if (Keyboard.current.qKey.isPressed) {
            mouseXInput = 1f;
        } else if (Keyboard.current.eKey.isPressed) {
            mouseXInput = -1f;
        }
        //myTransform.rotation = Quaternion.Euler(0f, value * maxRotationSpeed + myTransform.rotation.eulerAngles.y, 0f);
        HandleCameraRotation(delta, mouseXInput, mouseYInput);
    }

    public void ZoomCamera(float delta, float mouseYInput)
    {
        float value = -mouseYInput / 100f;
        if (Mathf.Abs(value) > 0.1f)
        {
            zoomHeight = myTransform.localPosition.y + value * stepSize;
            if (zoomHeight < minHeight)
            {
                zoomHeight = minHeight;
            }
            else if (zoomHeight > maxHeight)
            {
                zoomHeight = maxHeight;
            }
        }

        //cameraTransform.LookAt(targetTransform);

    }

    public void UpdateCameraPosition()
    {
        Vector3 zoomTarget = new Vector3(cameraPivotTransform.localPosition.x, zoomHeight, cameraPivotTransform.localPosition.z);
        zoomTarget -= zoomSpeed * (zoomHeight - cameraPivotTransform.localPosition.y) * Vector3.forward;
        cameraPivotTransform.localPosition = Vector3.Lerp(cameraPivotTransform.localPosition, zoomTarget, Time.deltaTime * zoomDamping);
        cameraPivotTransform.LookAt(targetTransform);
    }
}
