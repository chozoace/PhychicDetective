using UnityEngine;
using System.Collections;

public class PlayerControllerScript : MonoBehaviour
{
    [SerializeField] float _worldIsoAngle = 0.4625123f;
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

    private Vector2 _doorDir = Vector2.zero;

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

            if (yInput != 0 && xInput != 0)
            {
                _rigidBody.velocity = new Vector2(xInput * _moveSpeed * Mathf.Cos(_worldIsoAngle), yInput * _moveSpeed * Mathf.Sin(_worldIsoAngle));
            }
            else
                _rigidBody.velocity = new Vector2(xInput * _moveSpeed, yInput * _moveSpeed);
            anim.SetInteger("xSpeed", (int)xInput);
            anim.SetInteger("ySpeed", (int)yInput);
        }
    }

    public void levelTransitionFixedUpdate()
    {
        _rigidBody.velocity = new Vector2(_doorDir.x * _moveSpeed * Mathf.Cos(_worldIsoAngle), _doorDir.y * _moveSpeed * Mathf.Sin(_worldIsoAngle));
        anim.SetInteger("xSpeed", (int)_doorDir.x);
        anim.SetInteger("ySpeed", (int)_doorDir.y);
    }

    public IEnumerator movePlayerThroughRoom(Vector2 wallDir)
    {
        float startTime = Time.time;
        _doorDir = wallDir;
        GetComponent<BoxCollider2D>().enabled = false;

        while (true)
        {
            if(Time.time - startTime > .5)
            {
                _rigidBody.velocity = Vector2.zero;
                anim.SetInteger("xSpeed", 0);
                anim.SetInteger("ySpeed", 0);
                GetComponent<BoxCollider2D>().enabled = true;
                GameController.Instance().ChangeGameState(GameState._overworldState);
                yield break;
            }
            yield return null;
        }
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
                    anim.SetInteger("xSpeed", 0);
                    anim.SetInteger("ySpeed", 0);
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
}
