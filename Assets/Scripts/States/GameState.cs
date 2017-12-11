using UnityEngine;
using System.Collections;

public class GameState
{
    public static OverworldState _overworldState = new OverworldState();
    public static ConversationState _conversationState = new ConversationState();
    public static PauseState _pauseState = new PauseState();
    public static LevelChangeState _levelChangeState = new LevelChangeState();
    public static HistoryPauseState _historyPauseState = new HistoryPauseState();
    public static SettingsPauseState _settingsPauseState = new SettingsPauseState();

    protected string _stateName = "Default";
    public virtual string StateName { get { return _stateName; } }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void UpdateState() { }
    public virtual void FixedUpdateState() { }
}

