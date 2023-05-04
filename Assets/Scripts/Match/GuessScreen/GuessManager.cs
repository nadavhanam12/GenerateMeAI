using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static StateController;

public class GuessManager : AbstractStageChangeListener
{
    public static event Action<int, int> OnPlayerEarnedPoints;
    [SerializeField] private Canvas m_canvas;
    [SerializeField] List<GuessImageController> m_controllersList;
    [SerializeField] private GameObject m_questionMark;
    int m_playerId;
    public override void Init(MatchConfiguration matchConfiguration)
    {
        m_playerId = matchConfiguration.playerId;
        int index = 0;
        foreach (PlayerData playerData in matchConfiguration.PlayersData)
        {
            if (playerData.GetId() != m_playerId)
            {
                m_controllersList[index]?.Init(this, playerData.GetPlayerDetails());
                index++;
            }
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EnterPromptController.OnPlayerSubmitImage += PlayerSubmitImage;
        TurnController.OnPlayerNewTurn += PlayerNewTurn;
    }


    protected override void OnDisable()
    {
        base.OnDisable();
        EnterPromptController.OnPlayerSubmitImage -= PlayerSubmitImage;
        TurnController.OnPlayerNewTurn -= PlayerNewTurn;
    }

    public override void StateChange(StateController.MatchState state)
    {
        if (state == MatchState.WaitingForOtherPlayers)
            ToggleGuessControllers(false);
        else if (state == MatchState.Match)
        {
            ToggleGuessControllers(true);
            StartCoroutine("SimulateScores");
        }
        else if (state == MatchState.MatchFinish)
            ToggleInput(false);

    }
    private void PlayerNewTurn(int playerId)
    {
        if (m_playerId == playerId)
            ToggleInput(true);
        else
            ToggleInput(false);
    }

    private void ToggleInput(bool isOn)
    {
        foreach (GuessImageController imageController in m_controllersList)
            imageController.ToggleInput(isOn);
    }
    private void ToggleGuessControllers(bool toShow)
    {
        foreach (GuessImageController imageController in m_controllersList)
            imageController.gameObject.SetActive(toShow);
        m_questionMark.SetActive(!toShow);
    }
    private void PlayerSubmitImage(int playerId, GuessImageModel guessImageModel)
    {
        foreach (GuessImageController imageController in m_controllersList)
            if (imageController.GetPlayerId() == playerId)
                imageController.SetGuessImageModel(guessImageModel);
    }

    internal void AddPoints()
    {
        OnPlayerEarnedPoints(m_playerId, 100);
    }

    //simulates adding 100 points to different player every 0.X seconds
    IEnumerator SimulateScores()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.6f);
        yield return waitForSeconds;
        int playerId = 1;
        int pointsToAdd;
        for (int i = 0; i < 100; i++)
        {
            pointsToAdd = UnityEngine.Random.Range(1, 10);
            //if (playerId != 1)
            OnPlayerEarnedPoints(playerId, 10 * pointsToAdd);
            yield return waitForSeconds;
            playerId = playerId % 4;
            playerId++;
        }

    }

}
