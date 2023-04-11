using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private MatchInitializer m_matchInitializer;
    [SerializeField] private List<Texture> m_iconsList;
    void Start()
    {
        Init();
        StartMatch();
    }

    void Init()
    {
        MatchConfiguration matchConfiguration = GenerateMatchConfiguration();
        m_matchInitializer.Init(matchConfiguration);
    }

    void StartMatch()
    {
        m_matchInitializer.StartMatch();
    }


    MatchConfiguration GenerateMatchConfiguration()
    {
        MatchConfiguration matchConfiguration = new MatchConfiguration();
        //matchConfiguration.playerId = Random.Range(1, 5);
        matchConfiguration.playerId = 1;
        matchConfiguration.playersDetails = GeneratePlayerDetails();
        return matchConfiguration;

    }

    private List<PlayerDetails> GeneratePlayerDetails()
    {
        List<PlayerDetails> list = new List<PlayerDetails>();
        list.Add(new PlayerDetails(1, "Nadav", m_iconsList[0]));
        list.Add(new PlayerDetails(2, "Maria", m_iconsList[1]));
        list.Add(new PlayerDetails(3, "Omer", m_iconsList[2]));
        list.Add(new PlayerDetails(4, "Rotem", m_iconsList[3]));
        return list;
    }
}
