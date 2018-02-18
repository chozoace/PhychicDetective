using UnityEngine;
using System.Collections;

public class PlayerControllerScript : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 2;
    
    Rigidbody2D _rigidBody;
    public bool _hasBlueBox = false;
    Interactable _collidingInteractable;
    public Interactable CollidingInteractable { get { return _collidingInteractable; } set { _collidingInteractable = value; } }
    NotebookController _notebook;
    ItemDatabase _itemDatabase;
    static PlayerControllerScript _instance;
    private Animator anim;
    bool _controlsLocked = false;
    public bool ControlsLocked { get { return _controlsLocked; } set { _controlsLocked = value; } }

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(gameObject.transform.parent.gameObject);
        DontDestroyOnLoad(transform.parent.gameObject);
        _rigidBody = this.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public static PlayerControllerScript Instance()
    {
        return _instance;
    }

	void Start ()
    {
        _notebook = GetComponent<NotebookController>();
        _itemDatabase = GameObject.FindGameObjectWithTag("GameController").GetComponent<ItemDatabase>();
    }

    public void PlayerFixedUpdate()
    {
        if (!_controlsLocked)
        {
            float xInput = Input.GetAxisRaw("Horizontal");
            float yInput = Input.GetAxisRaw("Vertical");

            //commenting this allows for diagonal movement
            //if (yInput != 0)
            //xInput = 0;

            if (yInput != 0 && xInput != 0)
            {
                _rigidBody.velocity = new Vector2(xInput * _moveSpeed * Mathf.Cos(0.4625123f), yInput * _moveSpeed * Mathf.Sin(0.4625123f));
            }
            else
                _rigidBody.velocity = new Vector2(xInput * _moveSpeed, yInput * _moveSpeed);

            //this should not be dependent on input but by velocity of player
            anim.SetInteger("xSpeed", (int)xInput);
            anim.SetInteger("ySpeed", (int)yInput);
        }
    }

    public IEnumerator movePlayerThroughRoom(Vector2 wallDir)
    {
        Debug.Log("entered Coroutine");
        Vector2 destPoint = new Vector2();
        bool movementStarted = false;
        _controlsLocked = true;

        while (true)
        {
            //calc destPoint
            //set velocity once
            if(!movementStarted)
            {
                Debug.Log("starting movement");
                destPoint = new Vector2(.18f * wallDir.x, .65f * wallDir.y);
                _rigidBody.velocity = new Vector2(wallDir.x * _moveSpeed * Mathf.Cos(0.4625123f), wallDir.y * _moveSpeed * Mathf.Sin(0.4625123f));
                Debug.Log("setting velocity " + _rigidBody.velocity);
                Debug.Log("start point: " + transform.position + " end point: " + destPoint);
                movementStarted = true;
            }
            _rigidBody.velocity = new Vector2(wallDir.x * _moveSpeed * Mathf.Cos(0.4625123f), wallDir.y * _moveSpeed * Mathf.Sin(0.4625123f));
            Debug.Log("velocity " + _rigidBody.velocity);
            //this should not be dependent on distance but by time passed
            if (Mathf.Abs(transform.position.x) - Mathf.Abs(destPoint.x) <.0001 && Mathf.Abs(transform.position.y) - Mathf.Abs(destPoint.y) < .001)
            {
                Debug.Log("area met: " + transform.position + "\n" + destPoint);
                _rigidBody.velocity = Vector2.zero;
                _controlsLocked = false;
                yield break;
            }
            yield return null;
        }
    }

    void FixedUpdate()
    {
        
    }

    public void PlayerUpdate()
    {
        if (!_controlsLocked)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                if (_collidingInteractable)
                {
                    _collidingInteractable.onInteract();
                    GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    anim.SetInteger("xSpeed", (int)_rigidBody.velocity.x);
                    anim.SetInteger("ySpeed", (int)_rigidBody.velocity.y);
                }
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                GameController.Instance().ChangeGameState(GameState._pauseState);
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                GameController.Instance().ChangeGameState(GameState._settingsPauseState);
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            //convo history
            if (Input.GetKeyDown(KeyCode.C))
            {
                GameController.Instance().ChangeGameState(GameState._historyPauseState);
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            //Save Game
            if (Input.GetKeyDown(KeyCode.U))
            {
                Debug.Log("Saving game");
                GameController.Instance().SaveGame();
                _notebook.SaveData();
            }
            //Load Game
            if (Input.GetKeyDown(KeyCode.I))
            {
                Debug.Log("Loading game");
                GameController.Instance().LoadGame();
                _notebook.LoadData();
            }
        }
    }

    public void SaveData()
    {

    }

    public void CollectInteractable(int itemId)
    {
        Collectable item = _itemDatabase.FindCollectableWithId(itemId);
        if(item != null)
        {
            _notebook.AddEntry(item, item.Type);
        }
    }

    public bool InventoryContains(string type, int itemId)
    {
        return _notebook.NotebookContains(itemId, type);
    }

    void Update()
    {

    }
}
