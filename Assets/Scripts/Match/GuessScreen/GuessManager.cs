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
    }


    protected override void OnDisable()
    {
        base.OnDisable();
        EnterPromptController.OnPlayerSubmitImage -= PlayerSubmitImage;
    }


    public override void StateChange(StateController.MatchState state)
    {
        if (state == MatchState.Match)
            StartCoroutine("SimulateScores");
        else if (state == MatchState.MatchFinish)
            DisableInput();

    }

    private void DisableInput()
    {
        // foreach (GuessImageController imageController in m_controllersList)
        //     imageController.DisableInput();
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
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.5f);
        yield return waitForSeconds;
        int playerId = 1;
        int pointsToAdd;
        for (int i = 0; i < 100; i++)
        {
            pointsToAdd = UnityEngine.Random.Range(1, 6);
            OnPlayerEarnedPoints(playerId, 100 * pointsToAdd);
            yield return waitForSeconds;
            playerId = playerId % 4;
            playerId++;
        }

    }

}
