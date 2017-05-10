﻿using UnityEngine;
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

	// Use this for initialization
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
        
        if (yInput != 0)
            xInput = 0;
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
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().ChangeGameState(GameState._pauseState);
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        //Save Game
        if (Input.GetKeyDown(KeyCode.U))
        {
            GameController.Instance().SaveGame();
            _notebook.SaveData();
            if(GameObject.FindGameObjectWithTag("ConversationController").GetComponent<ConversationController>()._conversationInfo)
                GameObject.FindGameObjectWithTag("ConversationController").GetComponent<ConversationController>()._conversationInfo.SaveData();
        }
        //Load Game
        if (Input.GetKeyDown(KeyCode.I))
        {
            GameController.Instance().LoadGame();
            _notebook.LoadData();
        }
    }

    public void SaveData()
    {

    }

    public void CollectInteractable(int itemId)
    {
        //search for item by id then add to notebook
        Collectable item = _itemDatabase.FindCollectableWithId(itemId);
        Debug.Log("adding entry");
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
