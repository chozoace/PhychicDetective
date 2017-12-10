using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuState : GameState
{
    public override void Exit()
    {
        base.Exit();
    }

    public override void Enter()
    {
        if (_stateName == "Default")
        {
            _stateName = "MainMenuState";
            //_player = GameObject.FindGameObjectWithTag("Player");
        }

        base.Enter();
    }

    public override void UpdateState()
    {
        //base.UpdateState();
        //UPDATE MAINMENU CONTROLLER?
    }
}
