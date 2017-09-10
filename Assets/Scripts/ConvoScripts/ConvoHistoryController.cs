using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ConvoHistoryController : MonoBehaviour
{
    ConvoHistoryContainer _historyContainer;
    ConversationContainer _convoContainer;
    [SerializeField] TextAsset _historyFile;
    //reference to ui grid
    GameObject _historyConvoGrid;
    //prefab to create
    [SerializeField] GameObject _historyRecordPrefab;

    public void StartController()
    {
        if (_historyConvoGrid != null)
        {
            for (int i = 0; i < _historyConvoGrid.transform.childCount; i++)
            {
                GameObject.Destroy(_historyConvoGrid.transform.GetChild(i).gameObject);
            }
        }
        AssetDatabase.Refresh();
        _historyContainer = ConvoHistoryContainer.Load(_historyFile);
        if (_historyConvoGrid == null)
            _historyConvoGrid = GameObject.FindGameObjectWithTag("ConvoHistoryGrid");
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
    }

	public void UpdateMenu()
    {
        //control scroll bar here
        if (Input.GetKeyDown(KeyCode.W))
        {

        }
        else if (Input.GetKeyDown(KeyCode.S ))
        {

        }
        else if (Input.GetKeyDown(KeyCode.C))
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().ChangeGameState(GameState._overworldState);
    }
}