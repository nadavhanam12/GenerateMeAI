using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StateController;

public class EnemyController : AbstractStageChangeListener
{
    [SerializeField] Animator m_anim;
    [SerializeField] List<GameObject> m_enemys;
    public override void Init(MatchConfiguration matchConfiguration)
    {
        foreach (GameObject enemy in m_enemys)
        {
            enemy.SetActive(false);
        }
    }

    public override void StateChange(StateController.MatchState state)
    {
        if (state == MatchState.Match)
        {
            foreach (GameObject enemy in m_enemys)
            {
                enemy.SetActive(true);
            }
            m_anim.enabled = true;

        }
        else if (state == MatchState.MatchFinish)
            m_anim.enabled = false;
    }
}
