using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

public class HistoryPauseState : GameState
{
    GameObject _menuBackground;
    XmlDocument _xDoc = new XmlDocument();

    public void setMenuBackground(GameObject menuBackground)
    {
        _menuBackground = menuBackground;
        _menuBackground.SetActive(false);
    }

    public override void Enter()
    {
        if (_stateName == "Default")
            _stateName = "HistoryPauseState";
        
        _xDoc.Load("Assets/Resources/convoHistory.xml");
        _menuBackground.SetActive(true);
    }

    public override void Exit()
    {
        _menuBackground.SetActive(false);
        base.Exit();
    }

    public override void UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.C))
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().ChangeGameState(GameState._overworldState);
    }
}

[XmlRoot("ConvoHistory")]
class HistoryContainer
{

}

[System.Serializable]
class HistoryRecord
{
    
    public string _speaker;

    public string _speech;
}