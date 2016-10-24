using UnityEngine;
using System.Collections;

public class OverworldState : GameState
{
    GameObject _player = null;

    public override void Exit()
    {
        base.Exit();
    }

    public override void Enter()
    {
        if (_stateName == "Default")
        {
            _stateName = "OverworldState";
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        base.Enter();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        _player.GetComponent<PlayerControllerScript>().PlayerUpdate();
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
        _player.GetComponent<PlayerControllerScript>().PlayerFixedUpdate();
    }
}
