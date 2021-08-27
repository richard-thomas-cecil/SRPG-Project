using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;
    private PlayerControls controls;
    private PlayerInput playerInput;

    [SerializeField]private InputAction moveAction;

    private int frameWait;
    private int frameMoveWait;
    private int moveHold;

    private InputActionMap fieldBattleMap;

    private CharacterInfo selectedUnit;
       

    public TileInfo currentTile;

    public CharacterData character;

    private BasicMapInfo thisMap;

    void Awake()
    {
        rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
        frameWait = 0;
        frameMoveWait = 0;
        moveHold = 0;
        controls = new PlayerControls();
        playerInput = this.GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        fieldBattleMap = playerInput.actions.FindActionMap("FieldBattle");
    }

    // Start is called before the first frame update
    void Start()
    {
        thisMap = GameObject.Find("/MapInfo").GetComponent<BasicMapInfo>();
        currentTile = thisMap.startingPositions[0].GetComponent<TileInfo>();
        transform.position = currentTile.transform.position;

        //controls.FieldBattle.Move.started += ctx => MovePlayer();
        //controls.FieldBattle.Select.canceled += _ => WorldStateInfo.Instance.battleController.SelectTile();

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


    public void OnMove(InputValue inputValue)
    {
        Vector2 movement = inputValue.Get<Vector2>();



        if (moveAction.ReadValueAsObject() != null)
            MovePlayer(movement);
    }

    public void OnSelect()
    {
        WorldStateInfo.Instance.battleController.SelectTile();
    }

    public void OnBack()
    {
        WorldStateInfo.Instance.battleController.ReverseBattleState();
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

    public void MovePlayer()
    {
        Vector3 position = transform.position;

        Vector2 direction = moveAction.ReadValue<Vector2>();

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
