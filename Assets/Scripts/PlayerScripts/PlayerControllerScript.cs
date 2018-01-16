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
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");
        
        //commenting this allows for diagonal movement
        //if (yInput != 0)
           //xInput = 0;

        if(yInput != 0 && xInput != 0)
        {
            _rigidBody.velocity = new Vector2(xInput * _moveSpeed / 1.5f, yInput * _moveSpeed / 1.5f);
        }
        else
            _rigidBody.velocity = new Vector2(xInput * _moveSpeed, yInput * _moveSpeed);
        anim.SetInteger("xSpeed", (int)_rigidBody.velocity.x);
        anim.SetInteger("ySpeed", (int)_rigidBody.velocity.y);
    }

    void FixedUpdate()
    {
        
    }

    public void PlayerUpdate()
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
        if(Input.GetKeyDown(KeyCode.Return))
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
