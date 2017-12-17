using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class ConvoHistoryController : MonoBehaviour
{
    ConvoHistoryContainer _historyContainer;
    ConversationContainer _convoContainer;
    TextAsset _historyFile;
    //reference to ui grid
    GameObject _historyConvoGrid;
    //prefab to create
    [SerializeField] GameObject _historyRecordPrefab;
    Scrollbar _scrollBar;
    float _scrollIncreaseRate = .05f;

    public void StartController()
    {
        if (_historyConvoGrid != null)
        {
            for (int i = 0; i < _historyConvoGrid.transform.childCount; i++)
            {
                Destroy(_historyConvoGrid.transform.GetChild(i).gameObject);
                _scrollBar.value = 1;
                _scrollBar.numberOfSteps = 0;
            }
        }
        _historyContainer = ConvoHistoryContainer.Load("Assets/Resources/convoHistory.xml");
        if (_historyConvoGrid == null)
        {
            _historyConvoGrid = GameObject.FindGameObjectWithTag("ConvoHistoryGrid");
            _scrollBar = GameObject.FindGameObjectWithTag("HistoryScrollBar").GetComponent<Scrollbar>();
            _scrollBar.value = 1;
        }
        populateGrid();
    }

    void populateGrid()
    {
        foreach(Record record in _historyContainer._historyRecordList)
        {
            GameObject childObj = Instantiate(_historyRecordPrefab) as GameObject;
            childObj.GetComponent<HistoryRecordUIScript>().init();
            childObj.GetComponent<HistoryRecordUIScript>().setText(record._speech, record._speaker);
            _historyConvoGrid = GameObject.FindGameObjectWithTag("ConvoHistoryGrid");

            childObj.transform.SetParent(_historyConvoGrid.transform, false);
        }

        _scrollIncreaseRate = _scrollIncreaseRate * _scrollBar.size;
        Debug.Log("Scroll Rate: " + _scrollIncreaseRate);
    }

	public void UpdateMenu()
    {
        //control scroll bar here
        if (Input.GetKey(KeyCode.W))
        {
            if (_scrollBar.value < 1)
                _scrollBar.value += _scrollIncreaseRate;
        }
        else if (Input.GetKey(KeyCode.S ))
        {
            if (_scrollBar.value > 0)
                _scrollBar.value -= _scrollIncreaseRate;
        }
        else if (Input.GetKeyDown(KeyCode.C))
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().ChangeGameState(GameController.Instance().LastGameState);
    }
}