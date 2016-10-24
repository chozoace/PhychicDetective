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

	// Use this for initialization
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
            GameController.Instance().ChangeGameState(GameState._pauseState);
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    public void CollectInteractable(int itemId)
    {
        _hasBlueBox = true;
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
