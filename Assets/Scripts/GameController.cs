using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    /*
     * Game TO-DO: 
     * 2. Items that are interactable but don't disappear
     * 3. Main Menu, Settings Menu, New Game, Load Game, Save Game from Notebook?
     * 4. Sound effects on item interact, on page turn, Notebook open, pause menu
     *    menu click, as text loads
     * 6. How will Interrogations work? 
     * 7. Ability to use telepathy to gather information on a given statement
     * 8. On telepathy activate "Zoom into victims mind and see snapshot if available"
     */

    GameState _currentGameState = null;
    public GameState CurrentGameState { get { return _currentGameState; } }
    GameState _lastState;
    public GameState LastGameState { get { return _lastState; } }
    GameObject _playerDataPrefab = null;
    static GameController _instance;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
    }

    public static GameController Instance()
    {
        if(_instance != null)
            return _instance;
        else
            throw new System.ArgumentException("Game Controller instance is null");
    }
    
    void Start ()
    {
        StartGame();     
    }

    IEnumerator removeFadeScreen()
    {
        while (true)
        {
            CameraEffects.FadeToClear();

            if (CameraEffects.currentFadeAlpha() <= .005f)
            {
                ChangeGameState(GameState._overworldState);
                yield break;
            }
            yield return null;
        }
    }

    void StartGame ()
    {
        //start code should be moved here
        if (_playerDataPrefab == null)
        {
            _playerDataPrefab = (GameObject)Instantiate(Resources.Load("PlayerData"));
        }
        GetComponent<LevelController>().InitialLevelLoad(SceneManager.GetActiveScene().name, Vector2.zero);
        GameState._overworldState.AssignPlayer(_playerDataPrefab.transform.FindChild("Player").gameObject);
        _currentGameState = GameState._overworldState;
        _currentGameState.Enter();
        _lastState = _currentGameState;

        GameState._historyPauseState.setMenuBackground(GameObject.FindGameObjectWithTag("HistoryBackgroundMenu"));
        GameState._settingsPauseState.setPauseMenuObj(GameObject.Find("SettingsPauseObj"));

        if (GameObject.Find("BlackScreenFade(Clone)"))
        {
            ChangeGameState(GameState._levelChangeState);
            StartCoroutine("removeFadeScreen");
        }
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
        this.GetComponent<LevelController>().LoadGame();
        Vector2 playerPos;
        if (File.Exists(Application.persistentDataPath + "/GameController.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/GameController.dat", FileMode.Open);
            GameControllerSerialize iser = (GameControllerSerialize)bf.Deserialize(file);

            GetComponent<LevelController>().EndScene(iser._currentLevel, new Vector2(iser._xPos, iser._yPos));

            file.Close();
        }
    }

    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        if (File.Exists(Application.persistentDataPath + "/GameController.dat"))
            file = File.Open(Application.persistentDataPath + "/GameController.dat", FileMode.Open);
        else
            file = File.Open(Application.persistentDataPath + "/GameController.dat", FileMode.Create);

        GameControllerSerialize iser = new GameControllerSerialize();
        iser._xPos = PlayerControllerScript.Instance().transform.position.x;
        iser._yPos = PlayerControllerScript.Instance().transform.position.y;
        iser._currentLevel = GetComponent<LevelController>().GetCurrentLevel;

        bf.Serialize(file, iser);
        file.Close();

        this.GetComponent<LevelController>().SaveGame();
    }
}

[System.Serializable]
public class GameControllerSerialize
{
    public float _xPos;
    public float _yPos;
    public string _currentLevel;

    public GameControllerSerialize()
    {

    }
}
