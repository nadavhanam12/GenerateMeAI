using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StateController;

public class CharactersManager : AbstractStageChangeListener
{
    [SerializeField] List<Character> m_characters;
    [SerializeField] RectTransform[] m_destinationsPath;
    private int m_hiddenCharactersCount;

    public override void Init(MatchConfiguration matchConfiguration)
    {
        HidePath();
        ShowCharacters(false);
        m_hiddenCharactersCount = matchConfiguration.HiddenCharactersCount;
        for (int i = 0; i < matchConfiguration.PlayersData.Count; i++)
            if (m_characters.Count > i)
                m_characters[i].Init(
                    matchConfiguration.PlayersData[i].GetPlayerDetails(),
                    m_destinationsPath[0].localPosition);
    }

    void HidePath()
    {
        foreach (RectTransform dest in m_destinationsPath)
        {
            dest.gameObject.SetActive(false);
        }
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        GuessManager.OnPlayerEarnedPoints += OnPlayerEarnedPoint;
    }


    protected override void OnDisable()
    {
        base.OnDisable();
        GuessManager.OnPlayerEarnedPoints -= OnPlayerEarnedPoint;
    }
    public override void StateChange(StateController.MatchState state)
    {
        if (state == MatchState.MatchStarting)
            ShowCharacters(true);
        else if (state == MatchState.MatchFinish)
            ShowCharacters(false);
    }

    private void ShowCharacters(bool toShow)
    {
        foreach (Character character in m_characters)
            character.ToggleVisible(toShow);
    }

    public void OnPlayerEarnedPoint(int playerId, int points)
    {
        foreach (Character character in m_characters)
        {
            if (character.PlayerId == playerId)
            {
                character.AddPoints(points);
                Vector2 newDestination = GenerateDestination(character.Points);
                character.EnqueueNewDestination(newDestination);
                break;
            }
        }
    }

    private Vector2 GenerateDestination(int points)
    {
        float percentage = (float)points / (float)m_hiddenCharactersCount;
        if (percentage > 1f)
            percentage = 1f;

        int startIndex = Mathf.FloorToInt((m_destinationsPath.Length - 1) * percentage);
        int endIndex = Mathf.CeilToInt((m_destinationsPath.Length - 1) * percentage);

        Vector2 startPos = m_destinationsPath[startIndex].localPosition;
        Vector2 endPos = m_destinationsPath[endIndex].localPosition;

        float t = (percentage -
        (float)startIndex /
        (m_destinationsPath.Length - 1)) * (m_destinationsPath.Length - 1);

        Vector2 newDestination = Vector2.Lerp(startPos, endPos, t);
        //print(newDestination);
        return newDestination;
    }
}
