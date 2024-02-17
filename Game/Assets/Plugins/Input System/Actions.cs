//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/Plugins/Input System/Actions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @Actions : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @Actions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Actions"",
    ""maps"": [
        {
            ""name"": ""WASD"",
            ""id"": ""5116c59e-df45-4633-9a74-25de8028ded5"",
            ""actions"": [
                {
                    ""name"": ""ws"",
                    ""type"": ""Value"",
                    ""id"": ""40f3fd54-01b4-4360-8632-850cb17a4f77"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)"",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""ad"",
                    ""type"": ""Button"",
                    ""id"": ""126ac8bb-4f3e-4b12-b049-945cbbaf97c3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)"",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""d6d53a3d-74c1-46ce-8e83-765bf697212b"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ws"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""bba95645-f0ac-4288-a897-d5ea62240a1b"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ws"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""ef66593d-1474-464b-971d-d7b40a4f504f"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ws"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""f23b7763-fd9a-46bc-ae50-c191172609d3"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ad"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""2bf1c834-4ee3-4444-8eb0-3bd2a3a89827"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ad"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""9cc13c52-771e-4385-a356-364f093a0249"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ad"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""Abilities"",
            ""id"": ""61189763-4118-4908-a009-2a8d9444d979"",
            ""actions"": [
                {
                    ""name"": ""LaunchOut_Interact"",
                    ""type"": ""Button"",
                    ""id"": ""ffbb2a00-21bd-4d47-8d13-c764635a1807"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""LaunchIn"",
                    ""type"": ""Button"",
                    ""id"": ""f83c79e9-510d-4891-a5b6-0e18be9f925f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Punch"",
                    ""type"": ""Button"",
                    ""id"": ""d1e19c83-2637-412b-8840-ae57848564c9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Throw"",
                    ""type"": ""Button"",
                    ""id"": ""ba9ff580-91b2-48d5-b947-86c983c005dc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SlideSlam"",
                    ""type"": ""Button"",
                    ""id"": ""705848f4-4241-4480-8d22-da6bc0ef68e8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""e4612fea-b921-4ba4-b05c-409da8ed8e88"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Escape"",
                    ""type"": ""Button"",
                    ""id"": ""12450d40-3507-461a-a3b2-ad7b157aa51c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""5429c62b-2a51-40ee-b1fc-fbeb61bdf7df"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""aaa16187-4e59-4c2c-be3d-1971bccaebe3"",
                    ""path"": ""<Keyboard>/#(E)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LaunchOut_Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""45f14d8d-9e39-47d2-80c0-3f671f7d1d1e"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LaunchIn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c1f0782a-0711-4050-9906-ce78dddb01e6"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Punch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9c912e36-5fc8-4522-9eb5-d911145b26c4"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Throw"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""02fa8b67-0624-4b9e-afc8-0cec97eccbd0"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SlideSlam"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""310a3e57-01c0-4f40-9c2e-d053e979e9e8"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""feca3bef-ba90-41e7-8d46-78bd074728c1"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Escape"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bcee87c2-2e6f-4b57-960b-0101a191ce7b"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Basic"",
            ""id"": ""2b76b61c-f854-4948-a398-9257cc3e404d"",
            ""actions"": [
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""c1328a5f-787f-4415-8728-d5d73a27bb1d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ExtraShoot"",
                    ""type"": ""Button"",
                    ""id"": ""83007750-a240-4bf3-8de7-7167ce695f50"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f3a99945-7498-482a-9ed8-f85752438a91"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cf6688c5-d261-4bef-88b0-48fa4c44a522"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ExtraShoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // WASD
        m_WASD = asset.FindActionMap("WASD", throwIfNotFound: true);
        m_WASD_ws = m_WASD.FindAction("ws", throwIfNotFound: true);
        m_WASD_ad = m_WASD.FindAction("ad", throwIfNotFound: true);
        // Abilities
        m_Abilities = asset.FindActionMap("Abilities", throwIfNotFound: true);
        m_Abilities_LaunchOut_Interact = m_Abilities.FindAction("LaunchOut_Interact", throwIfNotFound: true);
        m_Abilities_LaunchIn = m_Abilities.FindAction("LaunchIn", throwIfNotFound: true);
        m_Abilities_Punch = m_Abilities.FindAction("Punch", throwIfNotFound: true);
        m_Abilities_Throw = m_Abilities.FindAction("Throw", throwIfNotFound: true);
        m_Abilities_SlideSlam = m_Abilities.FindAction("SlideSlam", throwIfNotFound: true);
        m_Abilities_Dash = m_Abilities.FindAction("Dash", throwIfNotFound: true);
        m_Abilities_Escape = m_Abilities.FindAction("Escape", throwIfNotFound: true);
        m_Abilities_Jump = m_Abilities.FindAction("Jump", throwIfNotFound: true);
        // Basic
        m_Basic = asset.FindActionMap("Basic", throwIfNotFound: true);
        m_Basic_Shoot = m_Basic.FindAction("Shoot", throwIfNotFound: true);
        m_Basic_ExtraShoot = m_Basic.FindAction("ExtraShoot", throwIfNotFound: true);
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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // WASD
    private readonly InputActionMap m_WASD;
    private IWASDActions m_WASDActionsCallbackInterface;
    private readonly InputAction m_WASD_ws;
    private readonly InputAction m_WASD_ad;
    public struct WASDActions
    {
        private @Actions m_Wrapper;
        public WASDActions(@Actions wrapper) { m_Wrapper = wrapper; }
        public InputAction @ws => m_Wrapper.m_WASD_ws;
        public InputAction @ad => m_Wrapper.m_WASD_ad;
        public InputActionMap Get() { return m_Wrapper.m_WASD; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(WASDActions set) { return set.Get(); }
        public void SetCallbacks(IWASDActions instance)
        {
            if (m_Wrapper.m_WASDActionsCallbackInterface != null)
            {
                @ws.started -= m_Wrapper.m_WASDActionsCallbackInterface.OnWs;
                @ws.performed -= m_Wrapper.m_WASDActionsCallbackInterface.OnWs;
                @ws.canceled -= m_Wrapper.m_WASDActionsCallbackInterface.OnWs;
                @ad.started -= m_Wrapper.m_WASDActionsCallbackInterface.OnAd;
                @ad.performed -= m_Wrapper.m_WASDActionsCallbackInterface.OnAd;
                @ad.canceled -= m_Wrapper.m_WASDActionsCallbackInterface.OnAd;
            }
            m_Wrapper.m_WASDActionsCallbackInterface = instance;
            if (instance != null)
            {
                @ws.started += instance.OnWs;
                @ws.performed += instance.OnWs;
                @ws.canceled += instance.OnWs;
                @ad.started += instance.OnAd;
                @ad.performed += instance.OnAd;
                @ad.canceled += instance.OnAd;
            }
        }
    }
    public WASDActions @WASD => new WASDActions(this);

    // Abilities
    private readonly InputActionMap m_Abilities;
    private IAbilitiesActions m_AbilitiesActionsCallbackInterface;
    private readonly InputAction m_Abilities_LaunchOut_Interact;
    private readonly InputAction m_Abilities_LaunchIn;
    private readonly InputAction m_Abilities_Punch;
    private readonly InputAction m_Abilities_Throw;
    private readonly InputAction m_Abilities_SlideSlam;
    private readonly InputAction m_Abilities_Dash;
    private readonly InputAction m_Abilities_Escape;
    private readonly InputAction m_Abilities_Jump;
    public struct AbilitiesActions
    {
        private @Actions m_Wrapper;
        public AbilitiesActions(@Actions wrapper) { m_Wrapper = wrapper; }
        public InputAction @LaunchOut_Interact => m_Wrapper.m_Abilities_LaunchOut_Interact;
        public InputAction @LaunchIn => m_Wrapper.m_Abilities_LaunchIn;
        public InputAction @Punch => m_Wrapper.m_Abilities_Punch;
        public InputAction @Throw => m_Wrapper.m_Abilities_Throw;
        public InputAction @SlideSlam => m_Wrapper.m_Abilities_SlideSlam;
        public InputAction @Dash => m_Wrapper.m_Abilities_Dash;
        public InputAction @Escape => m_Wrapper.m_Abilities_Escape;
        public InputAction @Jump => m_Wrapper.m_Abilities_Jump;
        public InputActionMap Get() { return m_Wrapper.m_Abilities; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(AbilitiesActions set) { return set.Get(); }
        public void SetCallbacks(IAbilitiesActions instance)
        {
            if (m_Wrapper.m_AbilitiesActionsCallbackInterface != null)
            {
                @LaunchOut_Interact.started -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnLaunchOut_Interact;
                @LaunchOut_Interact.performed -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnLaunchOut_Interact;
                @LaunchOut_Interact.canceled -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnLaunchOut_Interact;
                @LaunchIn.started -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnLaunchIn;
                @LaunchIn.performed -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnLaunchIn;
                @LaunchIn.canceled -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnLaunchIn;
                @Punch.started -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnPunch;
                @Punch.performed -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnPunch;
                @Punch.canceled -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnPunch;
                @Throw.started -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnThrow;
                @Throw.performed -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnThrow;
                @Throw.canceled -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnThrow;
                @SlideSlam.started -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnSlideSlam;
                @SlideSlam.performed -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnSlideSlam;
                @SlideSlam.canceled -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnSlideSlam;
                @Dash.started -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnDash;
                @Escape.started -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnEscape;
                @Escape.performed -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnEscape;
                @Escape.canceled -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnEscape;
                @Jump.started -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_AbilitiesActionsCallbackInterface.OnJump;
            }
            m_Wrapper.m_AbilitiesActionsCallbackInterface = instance;
            if (instance != null)
            {
                @LaunchOut_Interact.started += instance.OnLaunchOut_Interact;
                @LaunchOut_Interact.performed += instance.OnLaunchOut_Interact;
                @LaunchOut_Interact.canceled += instance.OnLaunchOut_Interact;
                @LaunchIn.started += instance.OnLaunchIn;
                @LaunchIn.performed += instance.OnLaunchIn;
                @LaunchIn.canceled += instance.OnLaunchIn;
                @Punch.started += instance.OnPunch;
                @Punch.performed += instance.OnPunch;
                @Punch.canceled += instance.OnPunch;
                @Throw.started += instance.OnThrow;
                @Throw.performed += instance.OnThrow;
                @Throw.canceled += instance.OnThrow;
                @SlideSlam.started += instance.OnSlideSlam;
                @SlideSlam.performed += instance.OnSlideSlam;
                @SlideSlam.canceled += instance.OnSlideSlam;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @Escape.started += instance.OnEscape;
                @Escape.performed += instance.OnEscape;
                @Escape.canceled += instance.OnEscape;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
            }
        }
    }
    public AbilitiesActions @Abilities => new AbilitiesActions(this);

    // Basic
    private readonly InputActionMap m_Basic;
    private IBasicActions m_BasicActionsCallbackInterface;
    private readonly InputAction m_Basic_Shoot;
    private readonly InputAction m_Basic_ExtraShoot;
    public struct BasicActions
    {
        private @Actions m_Wrapper;
        public BasicActions(@Actions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Shoot => m_Wrapper.m_Basic_Shoot;
        public InputAction @ExtraShoot => m_Wrapper.m_Basic_ExtraShoot;
        public InputActionMap Get() { return m_Wrapper.m_Basic; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BasicActions set) { return set.Get(); }
        public void SetCallbacks(IBasicActions instance)
        {
            if (m_Wrapper.m_BasicActionsCallbackInterface != null)
            {
                @Shoot.started -= m_Wrapper.m_BasicActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_BasicActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_BasicActionsCallbackInterface.OnShoot;
                @ExtraShoot.started -= m_Wrapper.m_BasicActionsCallbackInterface.OnExtraShoot;
                @ExtraShoot.performed -= m_Wrapper.m_BasicActionsCallbackInterface.OnExtraShoot;
                @ExtraShoot.canceled -= m_Wrapper.m_BasicActionsCallbackInterface.OnExtraShoot;
            }
            m_Wrapper.m_BasicActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
                @ExtraShoot.started += instance.OnExtraShoot;
                @ExtraShoot.performed += instance.OnExtraShoot;
                @ExtraShoot.canceled += instance.OnExtraShoot;
            }
        }
    }
    public BasicActions @Basic => new BasicActions(this);
    public interface IWASDActions
    {
        void OnWs(InputAction.CallbackContext context);
        void OnAd(InputAction.CallbackContext context);
    }
    public interface IAbilitiesActions
    {
        void OnLaunchOut_Interact(InputAction.CallbackContext context);
        void OnLaunchIn(InputAction.CallbackContext context);
        void OnPunch(InputAction.CallbackContext context);
        void OnThrow(InputAction.CallbackContext context);
        void OnSlideSlam(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnEscape(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
    }
    public interface IBasicActions
    {
        void OnShoot(InputAction.CallbackContext context);
        void OnExtraShoot(InputAction.CallbackContext context);
    }
}
