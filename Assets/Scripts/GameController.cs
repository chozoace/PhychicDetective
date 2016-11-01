﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    GameState _currentGameState = null;
    public GameState CurrentGameState { get { return _currentGameState; } }
    GameState _lastState;
    public GameObject _notebookMenu;

    //List of all interactables, should this be in overworldState?
    //These must all be prefabs, LevelController.SaveDate()
    //Create level controller which contains info on what stuff should be created
    //level controller should have list of all prefabs in level

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }
    
    void Start ()
    {
        _currentGameState = GameState._overworldState;
        _currentGameState.Enter();
        _lastState = _currentGameState;
    }

    public void ChangeGameState(GameState newState)
    {
        _currentGameState.Exit();
        _lastState = _currentGameState;
        _currentGameState = newState;
        _currentGameState.Enter();
    }

    // Update is called once per frame
    void Update ()
    {
        _currentGameState.UpdateState();
    }
    
    void FixedUpdate()
    {
        _currentGameState.FixedUpdateState();
    }

    public void LoadGame()
    {
        //Should load level info, where the player was, everything else loads on create
    }

    public void SaveGame()
    {

    }

    //needs to serialize player location, progress, level information, and all interactables convo info
    //progress should be stored in gamecontroller
    //Game controller should create player, notebook
}
