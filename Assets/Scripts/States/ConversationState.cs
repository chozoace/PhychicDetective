using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;

public class ConversationState : GameState
{
    GameObject _player = null;
    ConversationController _conversationController;
    bool _notebookControl = false;
    public bool NotebookControl { get { return _notebookControl; } set { _notebookControl = value; } }

    public override void Exit()
    {
        //end or save
        if(GameController.Instance().CurrentGameState.StateName.Equals("HistoryPauseState"))
        {
            _conversationController.saveConversation();
        }
        else
            _conversationController.EndConversation();
        base.Exit();
    }

    public override void Enter()
    {
        if (_stateName == "Default")
        {
            _stateName = "ConversationState";
            _player = GameObject.FindGameObjectWithTag("Player");
            _conversationController = GameObject.FindGameObjectWithTag("ConversationController").GetComponent<ConversationController>();
        }

        //start or continue
        if (GameController.Instance().LastGameState.StateName.Equals("HistoryPauseState"))
        {
            _conversationController.loadConversation();
        }
        else
            _conversationController.StartConversation();
    }

    public override void UpdateState()
    {
        if (_notebookControl == false)
            _conversationController.UpdateConversation();
        else
            NotebookController.Instance().CurrentPage.UpdatePage();
    }
}
