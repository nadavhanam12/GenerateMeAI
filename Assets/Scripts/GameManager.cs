using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private MatchInitializer m_matchInitializer;
    [SerializeField] private MatchConfiguration m_matchConfiguration;
    void Start()
    {
        Application.targetFrameRate = 60;
        Init();
        StartMatch();
    }

    void Init()
    {
        m_matchInitializer.Init(m_matchConfiguration);
    }

    void StartMatch()
    {
        m_matchInitializer.StartMatch();
    }


}
