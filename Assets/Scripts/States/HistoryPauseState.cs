using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class HistoryPauseState : GameState
{
    GameObject _menuBackground;
    ConvoHistoryController _historyControllerRef;

    public void setMenuBackground(GameObject menuBackground)
    {
        _menuBackground = menuBackground;
        _menuBackground.SetActive(false);
    }

    public override void Enter()
    {
        if (_stateName == "Default")
        {
            _stateName = "HistoryPauseState";
            _historyControllerRef = GameController.Instance().GetComponent<ConvoHistoryController>();
        }

        _historyControllerRef.StartController();
        _menuBackground.SetActive(true);
    }

    public override void Exit()
    {
        _menuBackground.SetActive(false);
        base.Exit();
    }

    public override void UpdateState()
    {
        _historyControllerRef.UpdateMenu();
    }
}