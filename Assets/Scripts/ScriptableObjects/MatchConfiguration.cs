using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MatchConfiguration", menuName = "ScriptableObjects/MatchConfiguration", order = 2)]
public class MatchConfiguration : ScriptableObject
{
    public int playerId;
    public List<PlayerData> PlayersData;

    public StagesDurations StagesDurations;
    public bool TurnMode = true;

}
