// GENERATED AUTOMATICALLY FROM 'Assets/Input/GrapplingHookControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @GrapplingHookControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @GrapplingHookControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GrapplingHookControls"",
    ""maps"": [
        {
            ""name"": ""GrapplingHook"",
            ""id"": ""31e60f6a-80cf-4d02-aa04-723bcbfb32da"",
            ""actions"": [
                {
                    ""name"": ""FireGrapplingHook"",
                    ""type"": ""Button"",
                    ""id"": ""5072f9b5-861d-4f9c-ae56-23020c5293a9"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ff833b6c-7eba-4ba3-a7f1-d8fd8ec8d97f"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FireGrapplingHook"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""375cd059-2ffd-4370-8bd6-3f0530e8d808"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FireGrapplingHook"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // GrapplingHook
        m_GrapplingHook = asset.FindActionMap("GrapplingHook", throwIfNotFound: true);
        m_GrapplingHook_FireGrapplingHook = m_GrapplingHook.FindAction("FireGrapplingHook", throwIfNotFound: true);
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

    // GrapplingHook
    private readonly InputActionMap m_GrapplingHook;
    private IGrapplingHookActions m_GrapplingHookActionsCallbackInterface;
    private readonly InputAction m_GrapplingHook_FireGrapplingHook;
    public struct GrapplingHookActions
    {
        private @GrapplingHookControls m_Wrapper;
        public GrapplingHookActions(@GrapplingHookControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @FireGrapplingHook => m_Wrapper.m_GrapplingHook_FireGrapplingHook;
        public InputActionMap Get() { return m_Wrapper.m_GrapplingHook; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GrapplingHookActions set) { return set.Get(); }
        public void SetCallbacks(IGrapplingHookActions instance)
        {
            if (m_Wrapper.m_GrapplingHookActionsCallbackInterface != null)
            {
                @FireGrapplingHook.started -= m_Wrapper.m_GrapplingHookActionsCallbackInterface.OnFireGrapplingHook;
                @FireGrapplingHook.performed -= m_Wrapper.m_GrapplingHookActionsCallbackInterface.OnFireGrapplingHook;
                @FireGrapplingHook.canceled -= m_Wrapper.m_GrapplingHookActionsCallbackInterface.OnFireGrapplingHook;
            }
            m_Wrapper.m_GrapplingHookActionsCallbackInterface = instance;
            if (instance != null)
            {
                @FireGrapplingHook.started += instance.OnFireGrapplingHook;
                @FireGrapplingHook.performed += instance.OnFireGrapplingHook;
                @FireGrapplingHook.canceled += instance.OnFireGrapplingHook;
            }
        }
    }
    public GrapplingHookActions @GrapplingHook => new GrapplingHookActions(this);
    public interface IGrapplingHookActions
    {
        void OnFireGrapplingHook(InputAction.CallbackContext context);
    }
}
