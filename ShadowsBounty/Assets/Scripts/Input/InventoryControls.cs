// GENERATED AUTOMATICALLY FROM 'Assets/Input/InventoryControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InventoryControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InventoryControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InventoryControls"",
    ""maps"": [
        {
            ""name"": ""Inventory"",
            ""id"": ""58f241e5-1d80-4b51-87aa-0c52df554242"",
            ""actions"": [
                {
                    ""name"": ""Item1"",
                    ""type"": ""Button"",
                    ""id"": ""d97f29de-343c-4dec-b054-e71d26ff144b"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Item2"",
                    ""type"": ""Button"",
                    ""id"": ""db82e072-91a3-4188-bd31-8c316e40f571"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Item3"",
                    ""type"": ""Button"",
                    ""id"": ""8033b5da-a22f-4790-b60a-ca923f85d90a"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Item4"",
                    ""type"": ""Button"",
                    ""id"": ""d8928e0e-16d8-4bc0-8bcd-51a2caca5f90"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ShopMenu"",
                    ""type"": ""Button"",
                    ""id"": ""bb577d95-598b-49a3-9059-04f7299e21bd"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""66980a7d-9aca-4bca-8578-51b5998d5d14"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Item1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fb87e092-0cd3-47ee-9d6e-7e483dcae985"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Item1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d0913e95-39b0-4784-a01d-67e51a78093d"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Item2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""142db220-471e-4907-aba2-06f80621b18a"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Item2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6d99df47-3059-42b7-a907-f2d6520524d0"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Item3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c4562a53-c1af-47f7-9df7-5895a14632f9"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Item3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7bbdb10d-bb3e-49a9-a9b3-63e74d20f0f6"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Item4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1265a720-da71-412f-87eb-267797da2bac"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Item4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""35215686-e2b2-42c4-b960-44b4901d6e61"",
                    ""path"": ""<Keyboard>/m"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ShopMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a5ef6ed5-4c22-4970-91cd-620a47af1325"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ShopMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Inventory
        m_Inventory = asset.FindActionMap("Inventory", throwIfNotFound: true);
        m_Inventory_Item1 = m_Inventory.FindAction("Item1", throwIfNotFound: true);
        m_Inventory_Item2 = m_Inventory.FindAction("Item2", throwIfNotFound: true);
        m_Inventory_Item3 = m_Inventory.FindAction("Item3", throwIfNotFound: true);
        m_Inventory_Item4 = m_Inventory.FindAction("Item4", throwIfNotFound: true);
        m_Inventory_ShopMenu = m_Inventory.FindAction("ShopMenu", throwIfNotFound: true);
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

    // Inventory
    private readonly InputActionMap m_Inventory;
    private IInventoryActions m_InventoryActionsCallbackInterface;
    private readonly InputAction m_Inventory_Item1;
    private readonly InputAction m_Inventory_Item2;
    private readonly InputAction m_Inventory_Item3;
    private readonly InputAction m_Inventory_Item4;
    private readonly InputAction m_Inventory_ShopMenu;
    public struct InventoryActions
    {
        private @InventoryControls m_Wrapper;
        public InventoryActions(@InventoryControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Item1 => m_Wrapper.m_Inventory_Item1;
        public InputAction @Item2 => m_Wrapper.m_Inventory_Item2;
        public InputAction @Item3 => m_Wrapper.m_Inventory_Item3;
        public InputAction @Item4 => m_Wrapper.m_Inventory_Item4;
        public InputAction @ShopMenu => m_Wrapper.m_Inventory_ShopMenu;
        public InputActionMap Get() { return m_Wrapper.m_Inventory; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InventoryActions set) { return set.Get(); }
        public void SetCallbacks(IInventoryActions instance)
        {
            if (m_Wrapper.m_InventoryActionsCallbackInterface != null)
            {
                @Item1.started -= m_Wrapper.m_InventoryActionsCallbackInterface.OnItem1;
                @Item1.performed -= m_Wrapper.m_InventoryActionsCallbackInterface.OnItem1;
                @Item1.canceled -= m_Wrapper.m_InventoryActionsCallbackInterface.OnItem1;
                @Item2.started -= m_Wrapper.m_InventoryActionsCallbackInterface.OnItem2;
                @Item2.performed -= m_Wrapper.m_InventoryActionsCallbackInterface.OnItem2;
                @Item2.canceled -= m_Wrapper.m_InventoryActionsCallbackInterface.OnItem2;
                @Item3.started -= m_Wrapper.m_InventoryActionsCallbackInterface.OnItem3;
                @Item3.performed -= m_Wrapper.m_InventoryActionsCallbackInterface.OnItem3;
                @Item3.canceled -= m_Wrapper.m_InventoryActionsCallbackInterface.OnItem3;
                @Item4.started -= m_Wrapper.m_InventoryActionsCallbackInterface.OnItem4;
                @Item4.performed -= m_Wrapper.m_InventoryActionsCallbackInterface.OnItem4;
                @Item4.canceled -= m_Wrapper.m_InventoryActionsCallbackInterface.OnItem4;
                @ShopMenu.started -= m_Wrapper.m_InventoryActionsCallbackInterface.OnShopMenu;
                @ShopMenu.performed -= m_Wrapper.m_InventoryActionsCallbackInterface.OnShopMenu;
                @ShopMenu.canceled -= m_Wrapper.m_InventoryActionsCallbackInterface.OnShopMenu;
            }
            m_Wrapper.m_InventoryActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Item1.started += instance.OnItem1;
                @Item1.performed += instance.OnItem1;
                @Item1.canceled += instance.OnItem1;
                @Item2.started += instance.OnItem2;
                @Item2.performed += instance.OnItem2;
                @Item2.canceled += instance.OnItem2;
                @Item3.started += instance.OnItem3;
                @Item3.performed += instance.OnItem3;
                @Item3.canceled += instance.OnItem3;
                @Item4.started += instance.OnItem4;
                @Item4.performed += instance.OnItem4;
                @Item4.canceled += instance.OnItem4;
                @ShopMenu.started += instance.OnShopMenu;
                @ShopMenu.performed += instance.OnShopMenu;
                @ShopMenu.canceled += instance.OnShopMenu;
            }
        }
    }
    public InventoryActions @Inventory => new InventoryActions(this);
    public interface IInventoryActions
    {
        void OnItem1(InputAction.CallbackContext context);
        void OnItem2(InputAction.CallbackContext context);
        void OnItem3(InputAction.CallbackContext context);
        void OnItem4(InputAction.CallbackContext context);
        void OnShopMenu(InputAction.CallbackContext context);
    }
}
