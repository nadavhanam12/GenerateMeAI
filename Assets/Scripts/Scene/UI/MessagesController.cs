using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static StateController;
using UnityEngine.SceneManagement;

public class MessagesController : AbstractStageChangeListener
{
    [SerializeField] private TMP_Text m_messageText;
    [SerializeField] float m_scaleUpValue = 2f;
    [SerializeField] float m_scaleUpDuration = 1f;
    public override void Init(MatchConfiguration matchConfiguration)
    {
        m_messageText.gameObject.SetActive(false);
    }

    public override void StateChange(StateController.MatchState state)
    {
        StopMessage();
        switch (state)
        {
            case MatchState.WaitingForOtherPlayers:
                StartMessage("Players Connecting...");
                break;
            case MatchState.MatchStarting:
                StartMessage("Get Ready");
                break;
        }
    }

    private void StopMessage()
    {
        LeanTween.cancel(m_messageText.gameObject);
        m_messageText.gameObject.SetActive(false);
    }

    void StartMessage(string message)
    {
        m_messageText.text = message;
        m_messageText.transform.localScale = Vector3.one;
        m_messageText.gameObject.SetActive(true);
        LeanTween.scale
            (m_messageText.gameObject, m_scaleUpValue * Vector3.one, m_scaleUpDuration)
            .setLoopPingPong()
            ;

    }
    public void OnRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
