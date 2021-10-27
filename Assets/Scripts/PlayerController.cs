using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;
    private PlayerControls controls;
    private PlayerInput playerInput;

    private Vector2 moveDirection;

    [SerializeField]private float playerSpeed;

    private int frameWait;
    private int frameMoveWait;
    private int moveHold;

    private InputActionMap fieldBattleMap;

    private CharacterInfo selectedUnit;

    #region ACTION_VARIABLES
    private InputAction baseMove;
    private InputAction fieldBattleMove;
    private InputAction select;
    private InputAction back;
    private InputAction openDetails;
    private InputAction moveAction;
    #endregion

    public TileInfo currentTile;

    public CharacterData character;

    private BasicMapInfo thisMap;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
        frameWait = 0;
        frameMoveWait = 0;
        moveHold = 0;
        controls = new PlayerControls();
        playerInput = this.GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        fieldBattleMap = playerInput.actions.FindActionMap("FieldBattle");

        SetActions();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (WorldStateInfo.Instance.currentMapInfo != null)
        {
            thisMap = GameObject.Find("/MapInfo").GetComponent<BasicMapInfo>();
            currentTile = thisMap.startingPositions[0].GetComponent<TileInfo>();
            transform.position = currentTile.transform.position;
        }

        //controls.FieldBattle.Move.started += ctx => MovePlayer();
        //controls.FieldBattle.Select.canceled += _ => WorldStateInfo.Instance.battleController.SelectTile();

    }

    private void SetActions()
    {
        fieldBattleMove = playerInput.actions.FindAction("Move");
        baseMove = playerInput.actions.FindAction("BaseMove");
        select = playerInput.actions.FindAction("Select");
        back = playerInput.actions.FindAction("Back");
        openDetails = playerInput.actions.FindAction("OpenDetails");
    }

    private void OnEnable()
    {
        //controls.Enable();
        //controls.UI.Disable();
        playerInput.actions.FindActionMap("UI").Disable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    public void OnAnimatorMove(InputAction.CallbackContext ctx)
    {
        Vector2 movement = ctx.ReadValue<Vector2>();

        MovePlayer(movement);
    }

    public void OnMove(InputValue inputValue)
    {
        moveDirection = inputValue.Get<Vector2>();

        //Vector2 movement = inputValue.Get<Vector2>();

        //if (moveAction.ReadValueAsObject() != null)
        //    MovePlayer(movement);
    }


    public void OnBaseMove(InputValue value)
    {
        moveDirection = value.Get<Vector2>();
    }

    public void OnSelect()
    {
        WorldStateInfo.Instance.battleController.SelectTile();
    }

    public void OnBack()
    {
        WorldStateInfo.Instance.battleController.ReverseBattleState();
    }

    public void OnOpenDetails()
    {
        WorldStateInfo.Instance.battleController.OpenUnitDetails();
    }
    //public void OnCancel()
    //{
    //    WorldStateInfo.Instance.battleController.ReverseBattleState();
    //}

    // Update is called once per frame
    void Update()
    {
    }


    void FixedUpdate()
    {
    //    if (moveAction.phase == InputActionPhase.Performed && fieldBattleMap.enabled)
    //    {
    //        float horizontal = controls.FieldBattle.Horizontal.ReadValue<float>();
    //        float vertical = controls.FieldBattle.Vertical.ReadValue<float>();

    //        MovePlayer(horizontal, vertical);
    //        frameMoveWait = frameMoveWait + 1;


    //    }
    //    else
    //    {
    //        moveHold = 0;
    //        frameMoveWait = 0;
    //    }
    //    frameWait = frameWait + 1;
    //    if (frameWait > 60)
    //    {
    //        frameWait = 0;
    //    }

        if(WorldStateInfo.Instance.playMode == PlayerMode.FIELD_BATTLE)
        {
            if (moveDirection != Vector2.zero)
            {
                MovePlayer(moveDirection.x, moveDirection.y);
                frameMoveWait = frameMoveWait + 1;
            }
            else
            {
                moveHold = 0;
                frameMoveWait = 0;
            }
        }
        if(WorldStateInfo.Instance.playMode == PlayerMode.BASE_MAP)
        {
            if(moveDirection != Vector2.zero)
                MoveInBase(moveDirection);
        }
    }

    public void SetControlsBasedOnPlayMode()
    {
        switch (WorldStateInfo.Instance.playMode)
        {
            case PlayerMode.FIELD_BATTLE:
                SwitchControlType("FieldBattle");
                break;
            case PlayerMode.BASE_MAP:
                SwitchControlType("BaseMap");
                break;
            default:
                break;
        }
    }


    public void SwitchControlType(string actionMap)
    {        
        //playerInput.currentActionMap.Disable();
        playerInput.SwitchCurrentActionMap(actionMap);
        playerInput.actions.FindActionMap(actionMap).Enable();
    }

    public void ToggleControls()
    {
        if(playerInput.currentActionMap.enabled)
        {
            playerInput.currentActionMap.Disable();
        }
        else
        {
            playerInput.currentActionMap.Enable();
        }
    }

    #region MOVEMENT
    public void MovePlayer(float horizontal, float vertical)
    {
        int movementWaitTime;

        Debug.Log(horizontal + ":" + vertical);

        if(moveHold >= 3)
        {
            movementWaitTime = 5;
        }
        else
        {
            movementWaitTime = 20;
        }
        if (frameMoveWait == 0 || frameMoveWait % movementWaitTime == 0)
        {
            //float horizontalMove = Input.GetAxisRaw("Horizontal");
            //float verticalMove = Input.GetAxisRaw("Vertical");

            Vector3 position = transform.position;

            TileInfo newTile = null;

            //position.x = position.x + (GetComponent<SpriteRenderer>().bounds.size.x * horizontalMove);
            //position.y = position.y + GetComponent<SpriteRenderer>().bounds.size.y * verticalMove;

            if(horizontal != 0)
            {
                if (currentTile.GetAdjacentTile(new Vector2(horizontal, 0)) != null)
                    newTile = currentTile.GetAdjacentTile(new Vector2(horizontal, 0)).GetComponent<TileInfo>();
            }
            else if(vertical != 0)
            {
                if(currentTile.GetAdjacentTile(new Vector2(0, vertical)) != null)
                    newTile = currentTile.GetAdjacentTile(new Vector2(0, vertical)).GetComponent<TileInfo>();
            }

            if(newTile != null)
            {
                position = newTile.transform.position;
                currentTile = newTile;
            }

            StartCoroutine(SmoothMovement(position));
                                    
            if(moveHold < 4)
            {
                moveHold = moveHold + 1;
            }
        }
    }

    public void MovePlayer(Vector2 direction)
    {
        Vector3 position = transform.position;

        TileInfo newTile = null;


        if (direction.x != 0)
        {
            if (currentTile.GetAdjacentTile(new Vector2(direction.x, 0)) != null)
                newTile = currentTile.GetAdjacentTile(new Vector2(direction.x, 0)).GetComponent<TileInfo>();
        }
        else if (direction.y != 0)
        {
            if (currentTile.GetAdjacentTile(new Vector2(0, direction.y)) != null)
                newTile = currentTile.GetAdjacentTile(new Vector2(0, direction.y)).GetComponent<TileInfo>();
        }

        if (newTile != null)
        {
            position = newTile.transform.position;
            currentTile = newTile;
        }

        StartCoroutine(SmoothMovement(position));
    }


    void MovePlayer(InputAction.CallbackContext context)
    {
        Vector3 position = transform.position;

        Vector2 direction = context.ReadValue<Vector2>();

        TileInfo newTile = null;


        if (direction.x != 0)
        {
            if (currentTile.GetAdjacentTile(new Vector2(direction.x, 0)) != null)
                newTile = currentTile.GetAdjacentTile(new Vector2(direction.x, 0)).GetComponent<TileInfo>();
        }
        else if (direction.y != 0)
        {
            if (currentTile.GetAdjacentTile(new Vector2(0, direction.y)) != null)
                newTile = currentTile.GetAdjacentTile(new Vector2(0, direction.y)).GetComponent<TileInfo>();
        }

        if (newTile != null)
        {
            position = newTile.transform.position;
            currentTile = newTile;
        }

        StartCoroutine(SmoothMovement(position));
    }

    public void MoveCursor(Vector3 newPosition, float speed)
    {
        StartCoroutine(SmoothMovement(newPosition, speed));
    }

    private void MoveInBase(Vector2 direction)
    {
        Vector3 position = transform.position;

        position = new Vector3(position.x + (direction.x * playerSpeed), position.y + (direction.y * playerSpeed));

        rigidBody2D.MovePosition(position);

        //StartCoroutine(SmoothMovement(position, playerSpeed));
    }

    //Function to move player directly to new part of base
    public void BaseTravelMovement(Vector2 newPosition)
    {
        transform.position = newPosition;
    }

    private IEnumerator SmoothMovement(Vector3 end, float speed = .25f)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rigidBody2D.position, end, speed);

            rigidBody2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            yield return null;
        }
    }
    #endregion

    
}
