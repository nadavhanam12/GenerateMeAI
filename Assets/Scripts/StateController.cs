using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class StateController : MonoBehaviour
{
    public enum State
    {
        ChoosingPrompt,
        WaitingForOtherPlayers,
        MatchStart,
        Match,
        MatchFinish,
    }

    public delegate void StateChange(State state);
    public static event StateChange OnStateChange;

    private State m_state;

    internal void Init()
    {
        SetState(State.ChoosingPrompt);

    }

    public async void StartMatch()
    {
        await Task.Delay(100);
        SetState(State.WaitingForOtherPlayers);
        await Task.Delay(100);
        SetState(State.MatchStart);
        await Task.Delay(100);
        SetState(State.Match);
    }

    private void SetState(State newState)
    {
        Debug.Log("OnStateChange: " + newState.ToString());
        m_state = newState;
        if (OnStateChange != null)
            OnStateChange(m_state);
    }

    internal void PlayerSubmitedData()
    {
        StartMatch();
    }
}
