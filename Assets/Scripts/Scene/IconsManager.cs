using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconsManager : AbstractStageChangeListener
{
    [SerializeField] private PlayerIcon[] m_iconsArray;

    public override void Init(MatchConfiguration matchConfiguration)
    {
        for (int i = 0; i < m_iconsArray.Length; i++)
        {
            m_iconsArray[i].Init(matchConfiguration.playersDetails[i]);
        }
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
        m_iconsArray[playerId - 1].AddScore(points);
    }
}
