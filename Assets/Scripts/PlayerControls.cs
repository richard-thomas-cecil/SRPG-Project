// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""FieldBattle"",
            ""id"": ""4e61b4d5-d6e0-4706-bbcf-d10dcd21d037"",
            ""actions"": [
                {
                    ""name"": ""Horizontal"",
                    ""type"": ""PassThrough"",
                    ""id"": ""c9ed3ab4-3234-4d53-8ac5-fcff0738eae4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Vertical"",
                    ""type"": ""PassThrough"",
                    ""id"": ""48b6c93b-aae5-44f6-92d6-5cc55b59c71f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""PassThrough"",
                    ""id"": ""1085b930-d8ad-433e-a5a1-8ba66beed14a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""5016a5ef-434e-4223-8c39-aa50ee7fef69"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Horizontal"",
                    ""id"": ""039dfb66-29fc-467d-90aa-7f282f2f839f"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""e5580218-bea2-4a7b-8aac-866437247ac5"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""5c4303b8-c158-4db0-b8cb-93cc4e123f11"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""90dec313-d349-4240-9b34-8f42c17b453e"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Vertical"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""f66012d2-b4f7-4320-8d31-0fe2d1808dd7"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Vertical"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""c5fbe37c-e7b5-49b8-bcea-a2524242191c"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Vertical"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""8308c988-0af5-4bfc-bd66-b0e13a568eb0"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Vertical"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""c95fa7bb-5325-4675-ad5a-a74f8dee56eb"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""New control scheme"",
            ""bindingGroup"": ""New control scheme"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // FieldBattle
        m_FieldBattle = asset.FindActionMap("FieldBattle", throwIfNotFound: true);
        m_FieldBattle_Horizontal = m_FieldBattle.FindAction("Horizontal", throwIfNotFound: true);
        m_FieldBattle_Vertical = m_FieldBattle.FindAction("Vertical", throwIfNotFound: true);
        m_FieldBattle_Select = m_FieldBattle.FindAction("Select", throwIfNotFound: true);
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

    // FieldBattle
    private readonly InputActionMap m_FieldBattle;
    private IFieldBattleActions m_FieldBattleActionsCallbackInterface;
    private readonly InputAction m_FieldBattle_Horizontal;
    private readonly InputAction m_FieldBattle_Vertical;
    private readonly InputAction m_FieldBattle_Select;
    public struct FieldBattleActions
    {
        private @PlayerControls m_Wrapper;
        public FieldBattleActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Horizontal => m_Wrapper.m_FieldBattle_Horizontal;
        public InputAction @Vertical => m_Wrapper.m_FieldBattle_Vertical;
        public InputAction @Select => m_Wrapper.m_FieldBattle_Select;
        public InputActionMap Get() { return m_Wrapper.m_FieldBattle; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(FieldBattleActions set) { return set.Get(); }
        public void SetCallbacks(IFieldBattleActions instance)
        {
            if (m_Wrapper.m_FieldBattleActionsCallbackInterface != null)
            {
                @Horizontal.started -= m_Wrapper.m_FieldBattleActionsCallbackInterface.OnHorizontal;
                @Horizontal.performed -= m_Wrapper.m_FieldBattleActionsCallbackInterface.OnHorizontal;
                @Horizontal.canceled -= m_Wrapper.m_FieldBattleActionsCallbackInterface.OnHorizontal;
                @Vertical.started -= m_Wrapper.m_FieldBattleActionsCallbackInterface.OnVertical;
                @Vertical.performed -= m_Wrapper.m_FieldBattleActionsCallbackInterface.OnVertical;
                @Vertical.canceled -= m_Wrapper.m_FieldBattleActionsCallbackInterface.OnVertical;
                @Select.started -= m_Wrapper.m_FieldBattleActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_FieldBattleActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_FieldBattleActionsCallbackInterface.OnSelect;
            }
            m_Wrapper.m_FieldBattleActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Horizontal.started += instance.OnHorizontal;
                @Horizontal.performed += instance.OnHorizontal;
                @Horizontal.canceled += instance.OnHorizontal;
                @Vertical.started += instance.OnVertical;
                @Vertical.performed += instance.OnVertical;
                @Vertical.canceled += instance.OnVertical;
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
            }
        }
    }
    public FieldBattleActions @FieldBattle => new FieldBattleActions(this);
    private int m_NewcontrolschemeSchemeIndex = -1;
    public InputControlScheme NewcontrolschemeScheme
    {
        get
        {
            if (m_NewcontrolschemeSchemeIndex == -1) m_NewcontrolschemeSchemeIndex = asset.FindControlSchemeIndex("New control scheme");
            return asset.controlSchemes[m_NewcontrolschemeSchemeIndex];
        }
    }
    public interface IFieldBattleActions
    {
        void OnHorizontal(InputAction.CallbackContext context);
        void OnVertical(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
    }
}
