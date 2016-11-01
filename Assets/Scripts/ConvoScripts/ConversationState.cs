using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;

public class ConversationState : GameState
{
    GameObject _player = null;
    //grab info from this
    ConversationController _conversationController;

    public override void Exit()
    {
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

        //start convo
        _conversationController.StartConversation();
        
        //base.Enter();
    }

    public override void UpdateState()
    {
        _conversationController.UpdateConversation();
        //base.UpdateState();
    }
}
