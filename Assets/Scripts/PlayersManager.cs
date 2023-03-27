using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StateController;

public class PlayersManager : MonoBehaviour
{
    [SerializeField] private PlayerData m_playerData1;
    [SerializeField] private PlayerData m_playerData2;
    [SerializeField] private PlayerData m_playerData3;
    [SerializeField] private PlayerData m_playerData4;

    void OnEnable()
    {
        StateController.OnStateChange += StateChange;
    }


    void OnDisable()
    {
        StateController.OnStateChange -= StateChange;
    }

    public void Init()
    {


    }


    private void StateChange(State state)
    {
        if (state == State.MatchStart)
            SimulateScores();
    }

    //simulates adding 100 points to different player every 0.X seconds
    IEnumerable SimulateScores()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.5f);
        while (true)
        {
            m_playerData1.AddPoints(100);
            return waitForSeconds;
        }

    }


}
