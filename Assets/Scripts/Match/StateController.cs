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
        WaitingForImages,
        MatchStarting,
        Match,
        MatchFinish,
    }
    public static event Action<MatchState> OnStateChange;

    private MatchState m_curState;
    private int m_playersCount;
    private int m_playersReady;
    protected virtual void OnEnable()
    {
        TimeController.OnStageTimeIsOver += StageTimeOver;
        GenerateImageController.OnPlayerSubmitImage += PlayerSubmitImage;

    }
    protected virtual void OnDisable()
    {
        TimeController.OnStageTimeIsOver -= StageTimeOver;
        GenerateImageController.OnPlayerSubmitImage -= PlayerSubmitImage;

    }

    private void PlayerSubmitImage
        (int playerId, GuessImageModel guessImageModel)
    {
        m_playersReady++;
        if (m_curState != MatchState.WaitingForImages)
            return;
        if (m_playersReady == m_playersCount)
            SetState((MatchState)((int)m_curState + 1));
    }

    public void StartMatch(int playersCount)
    {
        m_playersCount = playersCount;
        m_playersReady = 0;
        SetState(MatchState.WaitingForOtherPlayers);
        //SetState(MatchState.MatchStarting);

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
