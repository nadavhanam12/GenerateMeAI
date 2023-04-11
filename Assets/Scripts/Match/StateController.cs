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
    [SerializeField] private int m_choosingPromptDuration = 4;
    [SerializeField] private int m_MatchDuration = 40;

    public static event Action<MatchState> OnStateChange;

    private MatchState m_curState;

    public async void StartMatch()
    {
        SetState(MatchState.WaitingForOtherPlayers);
        await Task.Delay(1000);
        SetState(MatchState.ChoosingPrompt);
        await Task.Delay(1000 * m_choosingPromptDuration);
        SetState(MatchState.MatchStarting);
        await Task.Delay(1000 * m_MatchDuration);
        SetState(MatchState.Match);
        await Task.Delay(1000);
        SetState(MatchState.MatchFinish);
    }

    private void SetState(MatchState newState)
    {
        Debug.Log("OnStateChange: " + newState.ToString());
        m_curState = newState;
        OnStateChange?.Invoke(m_curState);
    }

}
