using UnityEngine;
using System.Collections;

public class PlayerControllerScript : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 2;

    bool _movingRight = false;
    bool _movingLeft = false;
    bool _movingUp = false;
    bool _movingDown = false;
    Rigidbody2D _rigidBody;
    public bool _hasBlueBox = false;
    Interactable _collidingInteractable;
    public Interactable CollidingInteractable { get { return _collidingInteractable; } set { _collidingInteractable = value; } }
    NotebookController _notebook;
    ItemDatabase _itemDatabase;
    static PlayerControllerScript instance;

	// Use this for initialization
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject.transform.parent.gameObject);
        DontDestroyOnLoad(transform.parent.gameObject);
    }

	void Start ()
    {
        _notebook = GetComponent<NotebookController>();
        _itemDatabase = GameObject.FindGameObjectWithTag("GameController").GetComponent<ItemDatabase>();
        _rigidBody = this.GetComponent<Rigidbody2D>();
    }

    public void PlayerFixedUpdate()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        _rigidBody.velocity = new Vector2(xInput * _moveSpeed, yInput * _moveSpeed);
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
            }
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().ChangeGameState(GameState._pauseState);
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        //Save Game
        if (Input.GetKeyDown(KeyCode.U))
        {
            _notebook.SaveData();
            GameObject.FindGameObjectWithTag("ConversationController").GetComponent<ConversationController>()._conversationInfo.SaveData();
        }
        //Load Game
        if (Input.GetKeyDown(KeyCode.I))
        {
            //_notebook.LoadData();
        }
    }

    public void CollectInteractable(int itemId)
    {
        //search for item by id then add to notebook
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
