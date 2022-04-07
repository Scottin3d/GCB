// GENERATED AUTOMATICALLY FROM 'Assets/InputSettings/CameraControlActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @CameraControlActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @CameraControlActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""CameraControlActions"",
    ""maps"": [
        {
            ""name"": ""CameraActionMap"",
            ""id"": ""b86c8506-13d0-468f-9802-e52a07dd8be1"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""0326415b-afe3-4263-b4d0-34ba71fd36f5"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RotateCamera"",
                    ""type"": ""Value"",
                    ""id"": ""d1b84bcb-aacf-4a28-bb06-aee7647ee55b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ZoomCamera"",
                    ""type"": ""Value"",
                    ""id"": ""368e70da-6541-4c5b-aec9-1965465d0c3a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""81adacfa-9ece-4f91-bbec-ebc5c1dcd2aa"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""58333f7f-444b-49e7-b8c5-98097ffab12f"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""611df088-c084-4750-ba05-a5a622929ee9"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ecb80270-2898-4d7b-936a-66174f61652f"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""2bd18504-7675-41ec-8b3e-857c5cafc68a"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""b2cd5890-fbf1-46b9-ba2d-1940b6d30c46"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e2f93baa-4187-4f52-936f-a25c0c31386c"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ZoomCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // CameraActionMap
        m_CameraActionMap = asset.FindActionMap("CameraActionMap", throwIfNotFound: true);
        m_CameraActionMap_Movement = m_CameraActionMap.FindAction("Movement", throwIfNotFound: true);
        m_CameraActionMap_RotateCamera = m_CameraActionMap.FindAction("RotateCamera", throwIfNotFound: true);
        m_CameraActionMap_ZoomCamera = m_CameraActionMap.FindAction("ZoomCamera", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // CameraActionMap
    private readonly InputActionMap m_CameraActionMap;
    private ICameraActionMapActions m_CameraActionMapActionsCallbackInterface;
    private readonly InputAction m_CameraActionMap_Movement;
    private readonly InputAction m_CameraActionMap_RotateCamera;
    private readonly InputAction m_CameraActionMap_ZoomCamera;
    public struct CameraActionMapActions
    {
        private @CameraControlActions m_Wrapper;
        public CameraActionMapActions(@CameraControlActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_CameraActionMap_Movement;
        public InputAction @RotateCamera => m_Wrapper.m_CameraActionMap_RotateCamera;
        public InputAction @ZoomCamera => m_Wrapper.m_CameraActionMap_ZoomCamera;
        public InputActionMap Get() { return m_Wrapper.m_CameraActionMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraActionMapActions set) { return set.Get(); }
        public void SetCallbacks(ICameraActionMapActions instance)
        {
            if (m_Wrapper.m_CameraActionMapActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnMovement;
                @RotateCamera.started -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnRotateCamera;
                @RotateCamera.performed -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnRotateCamera;
                @RotateCamera.canceled -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnRotateCamera;
                @ZoomCamera.started -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnZoomCamera;
                @ZoomCamera.performed -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnZoomCamera;
                @ZoomCamera.canceled -= m_Wrapper.m_CameraActionMapActionsCallbackInterface.OnZoomCamera;
            }
            m_Wrapper.m_CameraActionMapActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @RotateCamera.started += instance.OnRotateCamera;
                @RotateCamera.performed += instance.OnRotateCamera;
                @RotateCamera.canceled += instance.OnRotateCamera;
                @ZoomCamera.started += instance.OnZoomCamera;
                @ZoomCamera.performed += instance.OnZoomCamera;
                @ZoomCamera.canceled += instance.OnZoomCamera;
            }
        }
    }
    public CameraActionMapActions @CameraActionMap => new CameraActionMapActions(this);
    public interface ICameraActionMapActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnRotateCamera(InputAction.CallbackContext context);
        void OnZoomCamera(InputAction.CallbackContext context);
    }
}
