using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    [SerializeField] private PlayerDetails m_playerDetails;
    [SerializeField] private GuessImageModel m_guessImageModel;
    [SerializeField] private int m_points;
    [SerializeField] private Color m_color;

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
    public void InitScore()
    {
        m_points = 0;
    }


}
