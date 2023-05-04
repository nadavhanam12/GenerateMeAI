using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class StateController : MonoBehaviour
{
    public enum MatchState
    {
        WaitingForOtherPlayers,
        ChoosingPrompt,
        MatchStarting,
        Match,
        MatchFinish,
    }
    public static event Action<MatchState> OnStateChange;

    private MatchState m_curState;
    protected virtual void OnEnable()
    {
        TimeController.OnStageTimeIsOver += StageTimeOver;
    }
    protected virtual void OnDisable()
    {
        TimeController.OnStageTimeIsOver -= StageTimeOver;
    }

    public async void StartMatch()
    {
        await Task.Delay(1000);
        SetState(MatchState.WaitingForOtherPlayers);
    }
    private void StageTimeOver(MatchState finishedState)
    {
        SetState((MatchState)((int)m_curState + 1));

    }
    private void SetState(MatchState newState)
    {
        //Debug.Log("OnStateChange: " + newState.ToString());
        m_curState = newState;
        OnStateChange?.Invoke(m_curState);
    }

}
