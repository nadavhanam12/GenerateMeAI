using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StateController;

public class RunningPlane : MonoBehaviour
{

    Animator m_animator;

    void Start()
    {
        m_animator = GetComponent<Animator>();



    }

    void StateChange(State state)
    {
        if (state == State.MatchStart)
            m_animator.SetBool("shouldLoop", true);
        else if (state == State.MatchFinish)
            m_animator.SetBool("shouldLoop", false);
    }

    void OnEnable()
    {
        StateController.OnStateChange += StateChange;
    }


    void OnDisable()
    {
        StateController.OnStateChange -= StateChange;
    }
}
