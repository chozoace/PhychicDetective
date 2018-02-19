using UnityEngine;
using System.Collections;

public class LevelChangeState : GameState
{

    public override void Exit()
    {
        base.Exit();
    }

    public override void Enter()
    {
        if (_stateName == "Default")
        {
            _stateName = "LevelChangeState";
        }

        base.Enter();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
}
