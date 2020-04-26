// GENERATED AUTOMATICALLY FROM 'Assets/Input/ItemControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @ItemControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @ItemControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""ItemControls"",
    ""maps"": [
        {
            ""name"": ""Item"",
            ""id"": ""fc2f44f8-6812-4cf0-86d6-67248f4bef1d"",
            ""actions"": [
                {
                    ""name"": ""Use"",
                    ""type"": ""Button"",
                    ""id"": ""ae5f07fb-8751-4e02-acb1-96e29cd4fe64"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""04fa9a67-d0fc-48d1-b76b-f5ee6083cc8f"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Use"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""173a17eb-3f1d-47e0-826f-db52974b9f7e"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Use"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Item
        m_Item = asset.FindActionMap("Item", throwIfNotFound: true);
        m_Item_Use = m_Item.FindAction("Use", throwIfNotFound: true);
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

    // Item
    private readonly InputActionMap m_Item;
    private IItemActions m_ItemActionsCallbackInterface;
    private readonly InputAction m_Item_Use;
    public struct ItemActions
    {
        private @ItemControls m_Wrapper;
        public ItemActions(@ItemControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Use => m_Wrapper.m_Item_Use;
        public InputActionMap Get() { return m_Wrapper.m_Item; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ItemActions set) { return set.Get(); }
        public void SetCallbacks(IItemActions instance)
        {
            if (m_Wrapper.m_ItemActionsCallbackInterface != null)
            {
                @Use.started -= m_Wrapper.m_ItemActionsCallbackInterface.OnUse;
                @Use.performed -= m_Wrapper.m_ItemActionsCallbackInterface.OnUse;
                @Use.canceled -= m_Wrapper.m_ItemActionsCallbackInterface.OnUse;
            }
            m_Wrapper.m_ItemActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Use.started += instance.OnUse;
                @Use.performed += instance.OnUse;
                @Use.canceled += instance.OnUse;
            }
        }
    }
    public ItemActions @Item => new ItemActions(this);
    public interface IItemActions
    {
        void OnUse(InputAction.CallbackContext context);
    }
}
