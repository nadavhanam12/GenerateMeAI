using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class IconsManager : AbstractStageChangeListener
{
    [SerializeField] private PlayerIcon[] m_iconsArray;
    Dictionary<int, int> scores = new Dictionary<int, int>();
    public override void Init(MatchConfiguration matchConfiguration)
    {
        Vector2[] positions = GetPositionsArray();
        for (int i = 0; i < m_iconsArray.Length; i++)
        {
            m_iconsArray[i].Init(matchConfiguration.PlayersData[i].GetPlayerDetails(), positions);
            scores.Add(matchConfiguration.PlayersData[i].GetId(), 0);
        }
    }
    Vector2[] GetPositionsArray()
    {
        Vector2[] positions = new Vector2[m_iconsArray.Length];
        for (int i = 0; i < m_iconsArray.Length; i++)
        {
            positions[i] = (m_iconsArray[i].GetComponent<RectTransform>().anchoredPosition);
        }
        return positions;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        GuessManager.OnPlayerEarnedPoints += UpdateScore;
    }


    protected override void OnDisable()
    {
        base.OnDisable();
        GuessManager.OnPlayerEarnedPoints -= UpdateScore;
    }
    public override void StateChange(StateController.MatchState state)
    {
        //throw new System.NotImplementedException();
    }
    private void UpdateScore(int playerId, int points)
    {
        scores[playerId] += points;
        // sort the dictionary by value
        var sortedDict = scores.OrderByDescending(x => x.Value).ToList();

        int newScore;
        KeyValuePair<int, int> pair;
        for (int i = 0; i < sortedDict.Count; i++)
        {
            pair = sortedDict[i];
            newScore = pair.Value;
            m_iconsArray[pair.Key - 1].SetScore(newScore);
            m_iconsArray[pair.Key - 1].SetPos(i);
        }

    }
}
