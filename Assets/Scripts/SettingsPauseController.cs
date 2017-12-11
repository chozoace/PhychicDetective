using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPauseController : MonoBehaviour
{
    GameObject _cursor;
    List<GameObject> _cursorSelectionsList = new List<GameObject>();
    int _cursorIndex = 0;

    public void StartController()
    {
        if(_cursor == null)
        {
            _cursor = GameObject.Find("SettingsPauseCursor");
            foreach(Transform child in GameObject.Find("SettingsPauseCursorSelections").transform)
            {
                _cursorSelectionsList.Add(child.gameObject);
            }
        }
    }

    public void UpdateMenu()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (_cursorIndex < _cursorSelectionsList.Count - 1)
                moveCursor(_cursorIndex + 1);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            if (_cursorIndex > 0)
                moveCursor(_cursorIndex - 1);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            _cursorSelectionsList[_cursorIndex].GetComponent<IMenuItem>().execute();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            GameController.Instance().ChangeGameState(GameState._overworldState);
        }
    }

    void moveCursor(int newCursorIndex)
    {
        _cursorIndex = newCursorIndex;
        Transform t = _cursorSelectionsList[_cursorIndex].transform;
        float cursorWidth = _cursor.GetComponent<RectTransform>().rect.width / 250;
        Vector2 pos = new Vector2(t.position.x - (t.GetComponent<RectTransform>().rect.width / 440) -
            cursorWidth, t.position.y);
        _cursor.transform.position = pos;
    }
}
