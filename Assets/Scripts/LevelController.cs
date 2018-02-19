using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class LevelController : MonoBehaviour
{
    [SerializeField] List<Room> _prefabRoomList = new List<Room>();
    List<Room> _roomList = new List<Room>();
    Room _currentRoom;
    public string GetCurrentRoom { get { return _currentRoomInstance.GetRoomSceneName; } }
    //need get current scene
    public string GetCurrentScene { get { return SceneManager.GetActiveScene().name; } }
    public string GetCurrentLevel { get { return _currentRoom.GetRoomSceneName; } }
    GameObject _blackScreen;
    Color _currentAlphaColor;
    [SerializeField] float _fadeSpeed = 5f;
    bool _startScene = false;

    [SerializeField] List<Room> _roomInstanceList = new List<Room>();
    Room _currentRoomInstance;

    void Awake ()
    {
        foreach(Room room in _prefabRoomList)
        {
            //Room obj = Instantiate(room);
            _roomList.Add(room);
            room.gameObject.SetActive(false);
        }
    }

    public void EndScene(string newLevel, string newScene, Vector2 spawnVector)
    {
        _blackScreen = GameObject.FindGameObjectWithTag("PlayerData").transform.FindChild("BlackScreen").gameObject;
        Vector3 v = GameObject.FindGameObjectWithTag("PlayerData").transform.FindChild("Main Camera").position;
        v.z = 0;
        GameObject.FindGameObjectWithTag("PlayerData").transform.FindChild("BlackScreen").position = v;
        _currentAlphaColor = _blackScreen.GetComponent<SpriteRenderer>().color;
        _startScene = false;
        StartCoroutine(EndSceneRoutine(newLevel, newScene, spawnVector));
    }

    public void ChangeRooms(string nextRoom, Vector2 destPos, Vector2 wallDir)
    {
        Debug.Log("nextRoom: " + nextRoom);
        Room newRoom = null;

        //identify new room
        foreach (Room room in _roomInstanceList)
        {
            Debug.Log("Room name: " + room.GetRoomSceneName);
            if (room.GetRoomSceneName == nextRoom)
                newRoom = room;
        }

        if (newRoom != null)
        {
            StopAllCoroutines();
            //activate other room
            newRoom.gameObject.SetActive(true);
            //start fading for current and new room AND move player to new point
            StartCoroutine(newRoom.fadeRoomInRoutine());
            StartCoroutine(_currentRoomInstance.fadeRoomOutRoutine());
            //activatePlayerSpeed
            GameController.Instance().ChangeGameState(GameState._levelChangeState);
            GameState._levelChangeState._roomTransition = true;
            StartCoroutine(PlayerControllerScript.Instance().movePlayerThroughRoom(wallDir));
            _currentRoomInstance = newRoom;
        }
        Debug.Log("ended");
    }

    public void roomTransitionFinished()
    {

    }

    void FadeToBlack()
    {
        _blackScreen.GetComponent<SpriteRenderer>().color = Color.Lerp(_blackScreen.GetComponent<SpriteRenderer>().color, Color.black, _fadeSpeed * Time.deltaTime);
    }

    void FadeToClear()
    {
        _blackScreen.GetComponent<SpriteRenderer>().color = Color.Lerp(_blackScreen.GetComponent<SpriteRenderer>().color, Color.clear, _fadeSpeed * Time.deltaTime);
    }

    public IEnumerator EndSceneRoutine(string newLevel, string newScene, Vector2 spawnVector)
    {
        while(true)
        {
            FadeToBlack();

            if(_blackScreen.GetComponent<SpriteRenderer>().color.a >= .995f)
            {
                _blackScreen.GetComponent<SpriteRenderer>().color = Color.black;
                LoadLevel(newLevel, newScene, spawnVector);
                yield break;
            }
            else
            {
                yield return null;
            }
        }
    }

    public IEnumerator StartSceneRoutine()
    {
        yield return new WaitForSeconds(.001f);
        while (true)
        {
            FadeToClear();

            if (_blackScreen.GetComponent<SpriteRenderer>().color.a <= .005f)
            {
                _blackScreen.GetComponent<SpriteRenderer>().color = Color.clear;
                GameController.Instance().ChangeGameState(GameState._overworldState);
                yield break;
            }
            else
            {
                yield return null;
            }
        }
    }

    void LoadLevel(string newLevel, string newScene, Vector2 spawnVector)
    {
        _currentRoomInstance.gameObject.SetActive(false);
        if(!SceneManager.GetActiveScene().name.Equals(newScene))
            SceneManager.LoadScene(newScene, LoadSceneMode.Single);
        //load room
        foreach (Room room in _roomInstanceList)
        {
            Debug.Log("room name: " + room.GetRoomSceneName);
            if (room.GetRoomSceneName == newLevel)
                _currentRoomInstance = room;
            else
            {
                Debug.Log("set 0 for: " + room.GetRoomSceneName);
                room.setRoomObjOpacity(0);
                room.gameObject.SetActive(false);
            }
        }
        _currentRoomInstance.gameObject.SetActive(true);
        _currentRoomInstance.setRoomObjOpacity(255);
        GameObject.FindGameObjectWithTag("PlayerData").transform.position = spawnVector;
        foreach(Transform child in GameObject.FindGameObjectWithTag("PlayerData").transform)
        {
            Vector3 v = new Vector3(spawnVector.x, spawnVector.y, child.position.z);
            child.position = v;
        }

        StartCoroutine("StartSceneRoutine");
    }

    public void InitialLevelLoad(string level, Vector2 spawnVector)
    {
        foreach (Room room in _roomInstanceList)
        {
            if (room.GetRoomSceneName == level)
                _currentRoomInstance = room;
            else
            {
                room.setRoomObjOpacity(0);
                room.gameObject.SetActive(false);
                //fade out room contents
            }
        }
        _currentRoomInstance.gameObject.SetActive(true);
        //foreach (Room room in _roomList)
        //  if (room.GetRoomSceneName == level)
        //    _currentRoom = room;
        //_currentRoom.gameObject.SetActive(true);
    }

    void Update ()
    {

	}

    public void SaveGame()
    {
        foreach(Room room in _roomInstanceList)
        {
            room.SaveData();
        }
    }

    public void LoadGame()
    {
        foreach (Room room in _roomInstanceList)
        {
            room.LoadData();
        }
    }
}
