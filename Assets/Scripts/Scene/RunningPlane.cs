using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StateController;

public class RunningPlane : AbstractStageChangeListener
{

    Animator m_animator;

    public override void Init(MatchConfiguration matchConfiguration)
    {
        m_animator = GetComponent<Animator>();
    }

    public override void StateChange(MatchState state)
    {
        if (state == MatchState.MatchStarting)
            m_animator.SetBool("shouldLoop", true);
        else if (state == MatchState.MatchFinish)
            m_animator.SetBool("shouldLoop", false);
    }


}
