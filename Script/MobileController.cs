// GENERATED AUTOMATICALLY FROM 'Assets/Script/MobileController.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @MobileController : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @MobileController()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""MobileController"",
    ""maps"": [
        {
            ""name"": ""OneHand"",
            ""id"": ""9d55969e-207a-4961-853d-a640c87c2312"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""7551bd9a-f6b7-46de-8ec2-1dd7604da8c5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""9b045633-df1f-4bc0-a05a-939ab9963462"",
                    ""path"": ""<Pointer>/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // OneHand
        m_OneHand = asset.FindActionMap("OneHand", throwIfNotFound: true);
        m_OneHand_Move = m_OneHand.FindAction("Move", throwIfNotFound: true);
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

    // OneHand
    private readonly InputActionMap m_OneHand;
    private IOneHandActions m_OneHandActionsCallbackInterface;
    private readonly InputAction m_OneHand_Move;
    public struct OneHandActions
    {
        private @MobileController m_Wrapper;
        public OneHandActions(@MobileController wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_OneHand_Move;
        public InputActionMap Get() { return m_Wrapper.m_OneHand; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(OneHandActions set) { return set.Get(); }
        public void SetCallbacks(IOneHandActions instance)
        {
            if (m_Wrapper.m_OneHandActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_OneHandActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_OneHandActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_OneHandActionsCallbackInterface.OnMove;
            }
            m_Wrapper.m_OneHandActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
            }
        }
    }
    public OneHandActions @OneHand => new OneHandActions(this);
    public interface IOneHandActions
    {
        void OnMove(InputAction.CallbackContext context);
    }
}
