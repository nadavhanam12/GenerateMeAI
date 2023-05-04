using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharactersController : MonoBehaviour
{
    Dictionary<int, int> scores = new Dictionary<int, int>();
    [SerializeField] List<Character> m_characters;
    [SerializeField] float[] m_positionsArray;
    internal void Init(MatchConfiguration matchConfiguration)
    {
        for (int i = 0; i < m_characters.Count; i++)
        {
            m_characters[i].Init(matchConfiguration.PlayersData[i].GetPlayerDetails(), m_positionsArray);
            scores.Add(matchConfiguration.PlayersData[i].GetId(), 0);
        }
    }

    internal void AddPoints(int playerId, int points)
    {
        scores[playerId] += points;
        // sort the dictionary by value
        List<int> uniqueDescendingScores =
        scores.OrderByDescending(s => s.Value)
        .Select(pair => pair.Value)
        .Distinct()
        .ToList();

        int index;
        for (int i = 0; i < scores.Count; i++)
        {
            index = uniqueDescendingScores.IndexOf(scores[i + 1]);
            m_characters[i].SetPos(index);
        }
    }
}
