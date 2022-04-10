using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    private CameraController cameraController;
    private InputManager inputManager;

    #region MonoBehaviors
    private void Start()
    {
        cameraController = CameraController.singleton;
        inputManager = InputManager.singleton;
    }

    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;
        inputManager.TickInput(delta);
        if (cameraController != null)
        {
            cameraController.UpdateCameraController(delta);
        }
    }

    // used to reset flags
    private void LateUpdate()
    {
        inputManager.ResetFlags();
    }
    #endregion

    #region Public Functions
    #endregion

    #region Private Functions
    #endregion
}
