using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

public class ConvoHistoryController : MonoBehaviour
{
    ConvoHistoryContainer _historyContainer;
    ConversationContainer _convoContainer;
    [SerializeField] TextAsset historyFile;

    public void StartController()
    {
        _historyContainer = ConvoHistoryContainer.Load(historyFile);
        //create funciton clear ui grid and repopulate with gameobjs prefabs
    }

	public void UpdateMenu()
    {
        //control scroll bar here
        if (Input.GetKeyDown(KeyCode.C))
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().ChangeGameState(GameState._overworldState);
    }
}