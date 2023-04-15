
using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    [SerializeField] private PlayerDetails m_playerDetails;
    [SerializeField] private GuessImageModel m_guessImageModel;
    [SerializeField] private int m_points;

    public void SetPlayerDetails(PlayerDetails playerDetails)
    {
        m_playerDetails = playerDetails;
    }
    public void SetGuessImageModel(GuessImageModel guessImageModel)
    {
        m_guessImageModel = guessImageModel;
        m_points = 0;
    }

    public void AddPoints(int points)
    {
        m_points += points;
    }
    public int GetId()
    {
        return m_playerDetails.PlayerId;
    }
    public PlayerDetails GetPlayerDetails()
    {
        return m_playerDetails;
    }
    public void InitScore()
    {
        m_points = 0;
    }


}
