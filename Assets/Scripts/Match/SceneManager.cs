using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StateController;

public class SceneManager : AbstractStageChangeListener
{
    [SerializeField] private List<PlayerData> m_playerDataList;
    private int m_playerId;
    public override void Init(MatchConfiguration matchConfiguration)
    {
        m_playerId = matchConfiguration.playerId;
        int index = 0;
        for (int i = 0; i < 4; i++)
        {
            m_playerDataList[i].InitScore();
            m_playerDataList[i].SetPlayerDetails(matchConfiguration.playersDetails[i]);
            index++;
        }


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
        foreach (PlayerData data in m_playerDataList)
            if (data.GetId() == playerId)
                data.SetGuessImageModel(guessImageModel);
    }


    private void UpdateScore(int playerId, int points)
    {
        foreach (PlayerData data in m_playerDataList)
            if (data.GetId() == playerId)
                data.AddPoints(points);
    }


}
