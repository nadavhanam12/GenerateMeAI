using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static StateController;

public class TimeController : AbstractStageChangeListener
{
    public static event System.Action<MatchState> OnStageTimeIsOver;
    StagesDurations m_stagesDurations;
    [SerializeField] private TMP_Text m_timerText;
    int m_curTime;
    private MatchState m_curState;

    private WaitForSeconds waitOneSecond = new WaitForSeconds(1);


    public override void Init(MatchConfiguration matchConfiguration)
    {
        m_stagesDurations = matchConfiguration.StagesDurations;
    }

    public override void StateChange(StateController.MatchState state)
    {
        m_curState = state;
        switch (m_curState)
        {
            case MatchState.WaitingForOtherPlayers:
                StartTimer(m_stagesDurations.WaitingForPlayersDuration);
                break;
            case MatchState.ChoosingPrompt:
                StartTimer(m_stagesDurations.ChoosingPromptDuration);
                break;
            case MatchState.MatchStarting:
                StartTimer(m_stagesDurations.MatchStartingDuration);
                break;
            case MatchState.Match:
                StartTimer(m_stagesDurations.MatchDuration);
                break;
            case MatchState.MatchFinish:
                TurnOff();
                break;
        }
    }

    private void StartTimer(int duration)
    {
        StopAllCoroutines();
        m_curTime = duration;
        StartCoroutine(TimerCoroutine());
    }

    IEnumerator TimerCoroutine()
    {
        m_timerText.text = m_curTime.ToString();

        while (m_curTime >= 0)
        {
            yield return waitOneSecond;
            m_curTime--;
            m_timerText.text = m_curTime.ToString();
        }
        //Time Has Ended
        //Debug.Log("Time Has Ended");
        OnStageTimeIsOver?.Invoke(m_curState);
    }

    private void TurnOff()
    {
        StopAllCoroutines();
    }

}
