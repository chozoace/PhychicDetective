using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    static MainMenuController _instance;
    [SerializeField] GameObject _cursor;
    [SerializeField] GameObject _cursorSelectionsObj;
    [SerializeField] Camera _cam;
    List<GameObject> _cursorSelectionsList = new List<GameObject>();
    GameObject _blackScreen;
    float _fadeSpeed;
    int _cursorIndex = 0;

    //needs multiple screens (Pick save file, settings screen)

    void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

	void Start ()
    {
		foreach (Transform child in _cursorSelectionsObj.transform)
        {
            _cursorSelectionsList.Add(child.gameObject);
        }
	}

    public static MainMenuController Instance()
    {
        if (_instance != null)
            return _instance;
        else
            throw new System.ArgumentException("Main Menu Controller instance is null");
    }

    public void startGame()
    {
        StartCoroutine("startGameRoutine");
    }

    public IEnumerator startGameRoutine()
    {
        while(true)
        {
            CameraEffects.FadeToBlack();

            if(CameraEffects.currentFadeAlpha() >= .995f)
            {
                //call load level
                //Can either call game then load, or just start new game
                SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
                yield break;
            }
            yield return null;
        }
    }

    void Update ()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Current index: " + _cursorIndex);
            if(_cursorIndex < _cursorSelectionsList.Count - 1)
                moveCursor(_cursorIndex + 1);
        }
        else if(Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("Current index: " + _cursorIndex);
            if (_cursorIndex > 0)
                moveCursor(_cursorIndex - 1);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            _cursorSelectionsList[_cursorIndex].GetComponent<IMenuItem>().execute();
        }
    }

    void moveCursor(int newCursorIndex)
    {
        _cursorIndex = newCursorIndex;
        Transform t = _cursorSelectionsList[_cursorIndex].transform;
        float cursorWidth = _cursor.GetComponent<RectTransform>().rect.width / 100;
        Vector2 pos = new Vector2(t.position.x - (t.GetComponent<RectTransform>().rect.width / 200) -
            cursorWidth, t.position.y);
        _cursor.transform.position = pos;
    }
}