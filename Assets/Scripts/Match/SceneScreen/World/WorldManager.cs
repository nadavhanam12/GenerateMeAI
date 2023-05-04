using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StateController;

public class WorldManager : AbstractStageChangeListener
{
    private MatchConfiguration m_matchConfiguration;
    [SerializeField] CharactersController m_charactersController;
    private int m_playerId;
    public override void Init(MatchConfiguration matchConfiguration)
    {
        m_matchConfiguration = matchConfiguration;
        m_playerId = m_matchConfiguration.playerId;
        for (int i = 0; i < 4; i++)
        {
            m_matchConfiguration.PlayersData[i].InitScore();
        }
        m_charactersController.Init(matchConfiguration);

    }
    protected override void OnEnable()
    {
        base.OnEnable();
        EnterPromptController.OnPlayerSubmitImage += PlayerSubmitImage;
        GuessManager.OnPlayerEarnedPoints += UpdateScore;
    }


    protected override void OnDisable()
    {
        base.OnDisable();
        EnterPromptController.OnPlayerSubmitImage -= PlayerSubmitImage;
        GuessManager.OnPlayerEarnedPoints -= UpdateScore;
    }
    public override void StateChange(MatchState state)
    {

    }


    private void PlayerSubmitImage(int playerId, GuessImageModel guessImageModel)
    {
        foreach (PlayerData data in m_matchConfiguration.PlayersData)
            if (data.GetId() == playerId)
                data.SetGuessImageModel(guessImageModel);
    }


    private void UpdateScore(int playerId, int points)
    {
        foreach (PlayerData data in m_matchConfiguration.PlayersData)
            if (data.GetId() == playerId)
                data.AddPoints(points);

        m_charactersController.AddPoints(playerId, points);
    }


}
