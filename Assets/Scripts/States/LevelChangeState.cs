using UnityEngine;
using System.Collections;

public class LevelChangeState : GameState
{
    public bool _roomTransition = false;
    public override void Exit()
    {
        _roomTransition = false;
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

    public override void FixedUpdateState()
    {
        if(_roomTransition)
            PlayerControllerScript.Instance().levelTransitionFixedUpdate();
    }
}
