using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class LevelController : MonoBehaviour
{
    //stores level info
    //load level
    //save level
    //list of interactables
    //current level/room
    [SerializeField] List<Room> _roomList = new List<Room>();
    GameObject _blackScreen;
    Color _currentAlphaColor;
    [SerializeField] float _fadeSpeed = 5f;
    bool _startScene = false;

    //Controller should have list of levels
    //Every level should have a list of assets
    //levels should be serialized to see what is in it

    void Start ()
    {
        
    }

    public void EndScene(string newLevel, Vector2 spawnVector)
    {
        _blackScreen = GameObject.FindGameObjectWithTag("PlayerData").transform.FindChild("BlackScreen").gameObject;
        Vector3 v = GameObject.FindGameObjectWithTag("PlayerData").transform.FindChild("Main Camera").position;
        v.z = 0;
        GameObject.FindGameObjectWithTag("PlayerData").transform.FindChild("BlackScreen").position = v;
        _currentAlphaColor = _blackScreen.GetComponent<SpriteRenderer>().color;
        _startScene = false;
        StartCoroutine(EndSceneRoutine(newLevel,spawnVector));
    }

    void FadeToBlack()
    {
        _blackScreen.GetComponent<SpriteRenderer>().color = Color.Lerp(_blackScreen.GetComponent<SpriteRenderer>().color, Color.black, _fadeSpeed * Time.deltaTime);
    }

    void FadeToClear()
    {
        _blackScreen.GetComponent<SpriteRenderer>().color = Color.Lerp(_blackScreen.GetComponent<SpriteRenderer>().color, Color.clear, _fadeSpeed * Time.deltaTime);
    }

    public IEnumerator EndSceneRoutine(string newLevel, Vector2 spawnVector)
    {
        while(true)
        {
            FadeToBlack();

            if(_blackScreen.GetComponent<SpriteRenderer>().color.a >= .995f)
            {
                _blackScreen.GetComponent<SpriteRenderer>().color = Color.black;
                LoadLevel(newLevel, spawnVector);
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

    public void LoadLevel(string newLevel, Vector2 spawnVector)
    {
        
        SceneManager.LoadScene(newLevel, LoadSceneMode.Single);
        GameObject.FindGameObjectWithTag("PlayerData").transform.position = spawnVector;
        foreach(Transform child in GameObject.FindGameObjectWithTag("PlayerData").transform)
        {
            Vector3 v = new Vector3(spawnVector.x, spawnVector.y, child.position.z);
            child.position = v;
        }

        StartCoroutine("StartSceneRoutine");
    }

	void Update ()
    {
	    //if(_startScene)

	}

    public void SaveGame()
    {
        foreach(Room room in _roomList)
        {
            room.SaveData();
        }
    }

    public void LoadGame()
    {
        foreach (Room room in _roomList)
        {
            room.LoadData();
        }
    }
}
