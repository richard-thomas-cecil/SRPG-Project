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
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""1085b930-d8ad-433e-a5a1-8ba66beed14a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""5d58cbaa-386b-4d38-af6e-b7415949c41b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Back"",
                    ""type"": ""Button"",
                    ""id"": ""1ad9deb7-caee-4a68-a928-60167a2c207c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": ""NormalizeVector2"",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""OpenDetails"",
                    ""type"": ""Button"",
                    ""id"": ""c98a5e24-085a-48b3-b065-f4ec1574dafd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": ""NormalizeVector2"",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
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
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""3014ba64-1c2e-4571-9f87-fc7441f6ef3d"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""df140af7-9473-487f-b5b7-87b2119d3d69"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""f4927789-0f02-4959-bc84-d7783a4d2780"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""04cf0491-f57a-4204-886d-23564b2bf93e"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""8eac440c-041c-4e4c-8017-515b198c27d4"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""672f8d5d-5c01-452b-bf7f-40fe57080586"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9819ca75-ebd0-43fa-b9b2-f1998ce7496f"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""OpenDetails"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""c15b45db-5698-4d7b-81ed-5a37ea282e3e"",
            ""actions"": [
                {
                    ""name"": ""Navigate"",
                    ""type"": ""Value"",
                    ""id"": ""44b19a07-edd8-4b19-9053-0869e9a80ea6"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Submit"",
                    ""type"": ""Button"",
                    ""id"": ""5447c983-7c19-4730-915b-c2d0cf916bad"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""beecdf17-8df8-4d46-b393-1774ded3c0fc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Point"",
                    ""type"": ""PassThrough"",
                    ""id"": ""183a74ef-3146-4709-86ae-59319432c7f1"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Click"",
                    ""type"": ""PassThrough"",
                    ""id"": ""c2b49f28-e9e3-4673-b7a2-4abb02d4ef08"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ScrollWheel"",
                    ""type"": ""PassThrough"",
                    ""id"": ""260514dd-ba52-403e-b44a-a0f764acbc35"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MiddleClick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""0acc92a8-95f9-470a-8419-f8e4df49db2e"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightClick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""59a0acc1-1300-4bc6-9a35-1082cad25d08"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TrackedDevicePosition"",
                    ""type"": ""PassThrough"",
                    ""id"": ""1704ec9e-b5e0-4a20-a811-cd01e930b0bc"",
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TrackedDeviceOrientation"",
                    ""type"": ""PassThrough"",
                    ""id"": ""880b183f-031b-46e8-9821-7ef0ed6143c3"",
                    ""expectedControlType"": ""Quaternion"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Back"",
                    ""type"": ""Button"",
                    ""id"": ""dede5e87-fd3d-45f6-b5e8-d4ba52da0077"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""OpenDetails"",
                    ""type"": ""Button"",
                    ""id"": ""cb68b93a-fdf1-4fe2-9ea2-8f37c5fdeb64"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Gamepad"",
                    ""id"": ""1511054d-13b9-4eb6-8903-4d7fb9f61bc7"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""c353f5ea-43d7-465a-bc6b-558875d59654"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""0bd2bb1d-e928-4b81-9f57-8cecd132ab79"",
                    ""path"": ""<Gamepad>/rightStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""a5f191c7-2526-41c9-a533-128ad6a7973a"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""a5a9baf8-9a42-4a0d-b39a-c3f3ba10a7c7"",
                    ""path"": ""<Gamepad>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d728976c-ac19-4dea-bc36-423156babe8a"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a212a936-4589-4312-a2bd-7adf8737ecb2"",
                    ""path"": ""<Gamepad>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""8e7d44d2-6b66-4b72-868a-e16cedb4a631"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""f6b0cd52-732a-479e-b0d9-73f76cee4d2a"",
                    ""path"": ""<Gamepad>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""954f7f0a-caeb-47a7-8b6d-af953b05df85"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Joystick"",
                    ""id"": ""7e1aac5c-775c-490d-b4ae-f1cb71e85e3f"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""6bc45b51-67bb-418c-a797-954c7934da74"",
                    ""path"": ""<Joystick>/stick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""a48b9d3d-7817-4c31-8c04-dab03ba636ef"",
                    ""path"": ""<Joystick>/stick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""0cc0848b-9993-4ab1-b5f4-7d43041bc7bc"",
                    ""path"": ""<Joystick>/stick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""0592923d-958d-41f9-aaa1-c1f1da38c89c"",
                    ""path"": ""<Joystick>/stick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""190b5655-f04f-4bbb-a3f9-31be82514cbb"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""afab7ee2-4648-4a89-807e-71450d3d5af3"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Default"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""0f4e055b-fe15-400f-a129-67456675a9bf"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Default"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""19b8902b-9c3b-4dd1-a142-19961a2e299f"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Default"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""671d0366-ab8c-48a6-a0bd-8d3c2409902f"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Default"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ac327345-74f5-4d68-8654-7fc4f1802109"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Default"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""706cf369-8de0-4614-8b98-8693b6bdf098"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Default"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""32c94551-f921-4aa6-9063-fc67055a2eb7"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Default"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""e776d250-eaed-446f-8e99-b787981913b4"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Default"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""1f634fb6-f0ba-44f4-9d07-b08b4d6c8333"",
                    ""path"": ""*/{Submit}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""07348fff-8108-443d-8b3e-abc50e52fc6c"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Default"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9dbfc23f-7a6d-43fe-90a4-b793ebafd72e"",
                    ""path"": ""*/{Cancel}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ec65ad30-ce8f-4e35-97ba-13c3bcd741ea"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Default"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c536da56-4a60-4e00-893f-00bb517d24c5"",
                    ""path"": ""<Pen>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cda8db38-0d7c-4104-917b-38ae5d0a395d"",
                    ""path"": ""<Touchscreen>/touch*/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""109356a0-7a47-482d-83d7-ab09a8eb99a8"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Default"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a62ab7af-3960-425a-a783-71c272157ec1"",
                    ""path"": ""<Pen>/tip"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ab19814d-aaba-4638-8349-62ba76e66f3b"",
                    ""path"": ""<Touchscreen>/touch*/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e41bee00-24e0-4d07-ab6a-3519e31addd5"",
                    ""path"": ""<XRController>/trigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b46e8980-1d43-4e39-82ee-269d2e0edae7"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Default"",
                    ""action"": ""ScrollWheel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9d85a021-0169-4952-918d-a1bf11b59a14"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""MiddleClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ab39abea-b02d-444e-a018-d8b0cb2b1531"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""RightClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""06776645-c775-49b6-85e7-2a730c2595d7"",
                    ""path"": ""<XRController>/devicePosition"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""TrackedDevicePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""80e7ffb9-e3ce-4e41-a242-5ea2d2e71d37"",
                    ""path"": ""<XRController>/deviceRotation"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""TrackedDeviceOrientation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""06c637c2-5f19-4751-971a-4407d04a9e20"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b02635cd-4e7e-49a1-ae67-db9126b0448b"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""OpenDetails"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""BaseMap"",
            ""id"": ""6253cbd7-6982-4571-8301-ac978350210a"",
            ""actions"": [
                {
                    ""name"": ""BaseMove"",
                    ""type"": ""Value"",
                    ""id"": ""c54656e2-6483-4198-bb79-1fcd15edbfb3"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""OpenMenu"",
                    ""type"": ""Button"",
                    ""id"": ""2058547b-5b03-499d-889f-f89b07391881"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""OpenFastTravel"",
                    ""type"": ""Button"",
                    ""id"": ""c32f50a1-aec4-4235-be62-26804620aeea"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""e03ffb40-7a2d-4797-b1f2-0f1832b1410a"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""BaseMove"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""e03c75fa-7c3d-4e57-b60a-2ad5917c15ab"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""BaseMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""f0c8fbdf-3251-4898-abbb-19fbd44060fc"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""BaseMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a6d8edfa-5846-42f9-995d-bf2fa57841c6"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""BaseMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""ee7ccbc5-8891-4834-91d9-bdca3bbbcb8f"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""BaseMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""3a2b8170-e6e1-4e3d-904d-f60e47ad90f0"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""OpenMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c3e5b9eb-a832-456b-9626-056c9f057238"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""OpenFastTravel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Default"",
            ""bindingGroup"": ""Default"",
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
        m_FieldBattle_Select = m_FieldBattle.FindAction("Select", throwIfNotFound: true);
        m_FieldBattle_Move = m_FieldBattle.FindAction("Move", throwIfNotFound: true);
        m_FieldBattle_Back = m_FieldBattle.FindAction("Back", throwIfNotFound: true);
        m_FieldBattle_OpenDetails = m_FieldBattle.FindAction("OpenDetails", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_Navigate = m_UI.FindAction("Navigate", throwIfNotFound: true);
        m_UI_Submit = m_UI.FindAction("Submit", throwIfNotFound: true);
        m_UI_Cancel = m_UI.FindAction("Cancel", throwIfNotFound: true);
        m_UI_Point = m_UI.FindAction("Point", throwIfNotFound: true);
        m_UI_Click = m_UI.FindAction("Click", throwIfNotFound: true);
        m_UI_ScrollWheel = m_UI.FindAction("ScrollWheel", throwIfNotFound: true);
        m_UI_MiddleClick = m_UI.FindAction("MiddleClick", throwIfNotFound: true);
        m_UI_RightClick = m_UI.FindAction("RightClick", throwIfNotFound: true);
        m_UI_TrackedDevicePosition = m_UI.FindAction("TrackedDevicePosition", throwIfNotFound: true);
        m_UI_TrackedDeviceOrientation = m_UI.FindAction("TrackedDeviceOrientation", throwIfNotFound: true);
        m_UI_Back = m_UI.FindAction("Back", throwIfNotFound: true);
        m_UI_OpenDetails = m_UI.FindAction("OpenDetails", throwIfNotFound: true);
        // BaseMap
        m_BaseMap = asset.FindActionMap("BaseMap", throwIfNotFound: true);
        m_BaseMap_BaseMove = m_BaseMap.FindAction("BaseMove", throwIfNotFound: true);
        m_BaseMap_OpenMenu = m_BaseMap.FindAction("OpenMenu", throwIfNotFound: true);
        m_BaseMap_OpenFastTravel = m_BaseMap.FindAction("OpenFastTravel", throwIfNotFound: true);
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
    private readonly InputAction m_FieldBattle_Select;
    private readonly InputAction m_FieldBattle_Move;
    private readonly InputAction m_FieldBattle_Back;
    private readonly InputAction m_FieldBattle_OpenDetails;
    public struct FieldBattleActions
    {
        private @PlayerControls m_Wrapper;
        public FieldBattleActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Select => m_Wrapper.m_FieldBattle_Select;
        public InputAction @Move => m_Wrapper.m_FieldBattle_Move;
        public InputAction @Back => m_Wrapper.m_FieldBattle_Back;
        public InputAction @OpenDetails => m_Wrapper.m_FieldBattle_OpenDetails;
        public InputActionMap Get() { return m_Wrapper.m_FieldBattle; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(FieldBattleActions set) { return set.Get(); }
        public void SetCallbacks(IFieldBattleActions instance)
        {
            if (m_Wrapper.m_FieldBattleActionsCallbackInterface != null)
            {
                @Select.started -= m_Wrapper.m_FieldBattleActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_FieldBattleActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_FieldBattleActionsCallbackInterface.OnSelect;
                @Move.started -= m_Wrapper.m_FieldBattleActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_FieldBattleActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_FieldBattleActionsCallbackInterface.OnMove;
                @Back.started -= m_Wrapper.m_FieldBattleActionsCallbackInterface.OnBack;
                @Back.performed -= m_Wrapper.m_FieldBattleActionsCallbackInterface.OnBack;
                @Back.canceled -= m_Wrapper.m_FieldBattleActionsCallbackInterface.OnBack;
                @OpenDetails.started -= m_Wrapper.m_FieldBattleActionsCallbackInterface.OnOpenDetails;
                @OpenDetails.performed -= m_Wrapper.m_FieldBattleActionsCallbackInterface.OnOpenDetails;
                @OpenDetails.canceled -= m_Wrapper.m_FieldBattleActionsCallbackInterface.OnOpenDetails;
            }
            m_Wrapper.m_FieldBattleActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Back.started += instance.OnBack;
                @Back.performed += instance.OnBack;
                @Back.canceled += instance.OnBack;
                @OpenDetails.started += instance.OnOpenDetails;
                @OpenDetails.performed += instance.OnOpenDetails;
                @OpenDetails.canceled += instance.OnOpenDetails;
            }
        }
    }
    public FieldBattleActions @FieldBattle => new FieldBattleActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_Navigate;
    private readonly InputAction m_UI_Submit;
    private readonly InputAction m_UI_Cancel;
    private readonly InputAction m_UI_Point;
    private readonly InputAction m_UI_Click;
    private readonly InputAction m_UI_ScrollWheel;
    private readonly InputAction m_UI_MiddleClick;
    private readonly InputAction m_UI_RightClick;
    private readonly InputAction m_UI_TrackedDevicePosition;
    private readonly InputAction m_UI_TrackedDeviceOrientation;
    private readonly InputAction m_UI_Back;
    private readonly InputAction m_UI_OpenDetails;
    public struct UIActions
    {
        private @PlayerControls m_Wrapper;
        public UIActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Navigate => m_Wrapper.m_UI_Navigate;
        public InputAction @Submit => m_Wrapper.m_UI_Submit;
        public InputAction @Cancel => m_Wrapper.m_UI_Cancel;
        public InputAction @Point => m_Wrapper.m_UI_Point;
        public InputAction @Click => m_Wrapper.m_UI_Click;
        public InputAction @ScrollWheel => m_Wrapper.m_UI_ScrollWheel;
        public InputAction @MiddleClick => m_Wrapper.m_UI_MiddleClick;
        public InputAction @RightClick => m_Wrapper.m_UI_RightClick;
        public InputAction @TrackedDevicePosition => m_Wrapper.m_UI_TrackedDevicePosition;
        public InputAction @TrackedDeviceOrientation => m_Wrapper.m_UI_TrackedDeviceOrientation;
        public InputAction @Back => m_Wrapper.m_UI_Back;
        public InputAction @OpenDetails => m_Wrapper.m_UI_OpenDetails;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @Navigate.started -= m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
                @Navigate.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
                @Navigate.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
                @Submit.started -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                @Submit.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                @Submit.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                @Cancel.started -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                @Point.started -= m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
                @Point.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
                @Point.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
                @Click.started -= m_Wrapper.m_UIActionsCallbackInterface.OnClick;
                @Click.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnClick;
                @Click.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnClick;
                @ScrollWheel.started -= m_Wrapper.m_UIActionsCallbackInterface.OnScrollWheel;
                @ScrollWheel.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnScrollWheel;
                @ScrollWheel.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnScrollWheel;
                @MiddleClick.started -= m_Wrapper.m_UIActionsCallbackInterface.OnMiddleClick;
                @MiddleClick.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnMiddleClick;
                @MiddleClick.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnMiddleClick;
                @RightClick.started -= m_Wrapper.m_UIActionsCallbackInterface.OnRightClick;
                @RightClick.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnRightClick;
                @RightClick.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnRightClick;
                @TrackedDevicePosition.started -= m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDevicePosition;
                @TrackedDevicePosition.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDevicePosition;
                @TrackedDevicePosition.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDevicePosition;
                @TrackedDeviceOrientation.started -= m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDeviceOrientation;
                @TrackedDeviceOrientation.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDeviceOrientation;
                @TrackedDeviceOrientation.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDeviceOrientation;
                @Back.started -= m_Wrapper.m_UIActionsCallbackInterface.OnBack;
                @Back.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnBack;
                @Back.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnBack;
                @OpenDetails.started -= m_Wrapper.m_UIActionsCallbackInterface.OnOpenDetails;
                @OpenDetails.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnOpenDetails;
                @OpenDetails.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnOpenDetails;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Navigate.started += instance.OnNavigate;
                @Navigate.performed += instance.OnNavigate;
                @Navigate.canceled += instance.OnNavigate;
                @Submit.started += instance.OnSubmit;
                @Submit.performed += instance.OnSubmit;
                @Submit.canceled += instance.OnSubmit;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
                @Point.started += instance.OnPoint;
                @Point.performed += instance.OnPoint;
                @Point.canceled += instance.OnPoint;
                @Click.started += instance.OnClick;
                @Click.performed += instance.OnClick;
                @Click.canceled += instance.OnClick;
                @ScrollWheel.started += instance.OnScrollWheel;
                @ScrollWheel.performed += instance.OnScrollWheel;
                @ScrollWheel.canceled += instance.OnScrollWheel;
                @MiddleClick.started += instance.OnMiddleClick;
                @MiddleClick.performed += instance.OnMiddleClick;
                @MiddleClick.canceled += instance.OnMiddleClick;
                @RightClick.started += instance.OnRightClick;
                @RightClick.performed += instance.OnRightClick;
                @RightClick.canceled += instance.OnRightClick;
                @TrackedDevicePosition.started += instance.OnTrackedDevicePosition;
                @TrackedDevicePosition.performed += instance.OnTrackedDevicePosition;
                @TrackedDevicePosition.canceled += instance.OnTrackedDevicePosition;
                @TrackedDeviceOrientation.started += instance.OnTrackedDeviceOrientation;
                @TrackedDeviceOrientation.performed += instance.OnTrackedDeviceOrientation;
                @TrackedDeviceOrientation.canceled += instance.OnTrackedDeviceOrientation;
                @Back.started += instance.OnBack;
                @Back.performed += instance.OnBack;
                @Back.canceled += instance.OnBack;
                @OpenDetails.started += instance.OnOpenDetails;
                @OpenDetails.performed += instance.OnOpenDetails;
                @OpenDetails.canceled += instance.OnOpenDetails;
            }
        }
    }
    public UIActions @UI => new UIActions(this);

    // BaseMap
    private readonly InputActionMap m_BaseMap;
    private IBaseMapActions m_BaseMapActionsCallbackInterface;
    private readonly InputAction m_BaseMap_BaseMove;
    private readonly InputAction m_BaseMap_OpenMenu;
    private readonly InputAction m_BaseMap_OpenFastTravel;
    public struct BaseMapActions
    {
        private @PlayerControls m_Wrapper;
        public BaseMapActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @BaseMove => m_Wrapper.m_BaseMap_BaseMove;
        public InputAction @OpenMenu => m_Wrapper.m_BaseMap_OpenMenu;
        public InputAction @OpenFastTravel => m_Wrapper.m_BaseMap_OpenFastTravel;
        public InputActionMap Get() { return m_Wrapper.m_BaseMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BaseMapActions set) { return set.Get(); }
        public void SetCallbacks(IBaseMapActions instance)
        {
            if (m_Wrapper.m_BaseMapActionsCallbackInterface != null)
            {
                @BaseMove.started -= m_Wrapper.m_BaseMapActionsCallbackInterface.OnBaseMove;
                @BaseMove.performed -= m_Wrapper.m_BaseMapActionsCallbackInterface.OnBaseMove;
                @BaseMove.canceled -= m_Wrapper.m_BaseMapActionsCallbackInterface.OnBaseMove;
                @OpenMenu.started -= m_Wrapper.m_BaseMapActionsCallbackInterface.OnOpenMenu;
                @OpenMenu.performed -= m_Wrapper.m_BaseMapActionsCallbackInterface.OnOpenMenu;
                @OpenMenu.canceled -= m_Wrapper.m_BaseMapActionsCallbackInterface.OnOpenMenu;
                @OpenFastTravel.started -= m_Wrapper.m_BaseMapActionsCallbackInterface.OnOpenFastTravel;
                @OpenFastTravel.performed -= m_Wrapper.m_BaseMapActionsCallbackInterface.OnOpenFastTravel;
                @OpenFastTravel.canceled -= m_Wrapper.m_BaseMapActionsCallbackInterface.OnOpenFastTravel;
            }
            m_Wrapper.m_BaseMapActionsCallbackInterface = instance;
            if (instance != null)
            {
                @BaseMove.started += instance.OnBaseMove;
                @BaseMove.performed += instance.OnBaseMove;
                @BaseMove.canceled += instance.OnBaseMove;
                @OpenMenu.started += instance.OnOpenMenu;
                @OpenMenu.performed += instance.OnOpenMenu;
                @OpenMenu.canceled += instance.OnOpenMenu;
                @OpenFastTravel.started += instance.OnOpenFastTravel;
                @OpenFastTravel.performed += instance.OnOpenFastTravel;
                @OpenFastTravel.canceled += instance.OnOpenFastTravel;
            }
        }
    }
    public BaseMapActions @BaseMap => new BaseMapActions(this);
    private int m_DefaultSchemeIndex = -1;
    public InputControlScheme DefaultScheme
    {
        get
        {
            if (m_DefaultSchemeIndex == -1) m_DefaultSchemeIndex = asset.FindControlSchemeIndex("Default");
            return asset.controlSchemes[m_DefaultSchemeIndex];
        }
    }
    public interface IFieldBattleActions
    {
        void OnSelect(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnBack(InputAction.CallbackContext context);
        void OnOpenDetails(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnNavigate(InputAction.CallbackContext context);
        void OnSubmit(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
        void OnPoint(InputAction.CallbackContext context);
        void OnClick(InputAction.CallbackContext context);
        void OnScrollWheel(InputAction.CallbackContext context);
        void OnMiddleClick(InputAction.CallbackContext context);
        void OnRightClick(InputAction.CallbackContext context);
        void OnTrackedDevicePosition(InputAction.CallbackContext context);
        void OnTrackedDeviceOrientation(InputAction.CallbackContext context);
        void OnBack(InputAction.CallbackContext context);
        void OnOpenDetails(InputAction.CallbackContext context);
    }
    public interface IBaseMapActions
    {
        void OnBaseMove(InputAction.CallbackContext context);
        void OnOpenMenu(InputAction.CallbackContext context);
        void OnOpenFastTravel(InputAction.CallbackContext context);
    }
}
