using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    GameState _currentGameState = null;
    public GameState CurrentGameState { get { return _currentGameState; } }
    GameState _lastState;
    public GameObject _notebookMenu;

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
}
