using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StateController;

public class TurnController : AbstractStageChangeListener
{
    public static event Action<int> OnPlayerNewTurn;

    List<PlayerDetails> players;
    WaitForSeconds waitForSeconds;
    int m_playerId;
    public override void Init(MatchConfiguration matchConfiguration)
    {
        if (!matchConfiguration.TurnMode)
        {
            this.enabled = false;
            return;
        }
        m_playerId = matchConfiguration.playerId;
        waitForSeconds = new WaitForSeconds(matchConfiguration.StagesDurations.TurnDuration);
        players = new List<PlayerDetails>();
        foreach (PlayerData data in matchConfiguration.PlayersData)
            players.Add(data.GetPlayerDetails());
        Shuffle(players);
    }

    public override void StateChange(StateController.MatchState state)
    {
        if (state == MatchState.Match)
            StartCoroutine(StartTurns());
        else if (state == MatchState.MatchFinish)
            StopAllCoroutines();
    }

    IEnumerator StartTurns()
    {
        yield return new WaitForSeconds(1);
        int turn = 0;
        for (int i = 0; i < 100; i++)
        {
            InvokeTurn(players[turn % 4]);
            yield return waitForSeconds;
            if (players[turn % 4].PlayerId == m_playerId)
            {
                yield return waitForSeconds;
                yield return waitForSeconds;
                yield return waitForSeconds;
            }
            turn++;
        }
    }
    private void InvokeTurn(PlayerDetails playerDetails)
    {
        Debug.Log("Turn of " + playerDetails.PlayerName);
        OnPlayerNewTurn?.Invoke(playerDetails.PlayerId);
    }

    public void Shuffle(List<PlayerDetails> list)
    {
        System.Random random = new System.Random();
        // Loop through the list and swap each item with a random item
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = random.Next(i, list.Count);
            var temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
